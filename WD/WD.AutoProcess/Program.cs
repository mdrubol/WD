using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.IO;
using System.Collections.Generic;
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Configuration;
using System.Text.RegularExpressions;
namespace WD.AutoProcess
{
    /// <summary>
    /// 
    /// </summary>
    public class AppConfiguration
    {
        public const int CYCLE = 60000;//1 minute cycle
        public const int INSERT = 60000;//1 minute insertion cycle
        public const int WAIT =120000;//2 minute delay before starting proce ss
        public const int MINUTES = 59;// minutes
        public const int SECONDS = 0;
        public static string SERVER { get { return ConfigurationManager.AppSettings.Get("TelnetServer"); } }
        public static string USERNAME { get { return ConfigurationManager.AppSettings.Get("TelnetUserName"); } }
        public static string PASSWORD { get { return ConfigurationManager.AppSettings.Get("TelnetPassword"); } }
        public static int PORT { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(ConfigurationManager.AppSettings.Get("TelnetPort"), 22); } }
        public static string FILEPATH { get { return ConfigurationManager.AppSettings.Get("UnixScriptPath"); } }
        public static string CYCLEEND { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<string>(System.Configuration.ConfigurationManager.AppSettings.Get("CycleEnd"), "08:00 PM"); } }
        public static string CYCLESTART { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<string>(System.Configuration.ConfigurationManager.AppSettings.Get("CycleStart"), "08:00 AM"); } }
        public static int CYCLE1INTERVAL { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(System.Configuration.ConfigurationManager.AppSettings.Get("Cycle1Interval"), 60000); } }
        public static int CYCLE2INTERVAL { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(System.Configuration.ConfigurationManager.AppSettings.Get("Cycle2Interval"), 60000); } }
        public static string EMAILORSMS { get { return System.Configuration.ConfigurationManager.AppSettings.Get("EmailOrSms").ToLower(); } }
        public static int DELAY { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(System.Configuration.ConfigurationManager.AppSettings.Get("Delay"), 30 * 60 * 1000); } }//28 minute delay
        public static int RUNNINGDELAY { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(System.Configuration.ConfigurationManager.AppSettings.Get("RunningDelay"), 60000); } }//28 minute delay
        public static int KILLDELAY { get { return WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(System.Configuration.ConfigurationManager.AppSettings.Get("KillDelay"), 20000); } }//28 minute delay
    }
    #region TelnetConnection - no need to edit
    /// <summary>
    /// Supports telnet connectivity.
    /// <list type="bullet">
    /// <item>Version 0.70: 1st running version</item>
    /// <item>Version 0.71: Telnet class renamed to Terminal and close method improved</item>
    /// <item>Version 0.72: Added custom exceptions which may be used externally, Feedback of Mark H. considered, Wait() method added and <see cref="WaitForChangedScreen(int)"></see> fixed.</item>
    ///	<item>Version 0.73:	Offset problem in Virtual Screen fixed due to mail of Steve, thanks!</item>
    ///	<item>Version 0.74:	SendResponseFunctionKey(int) and fixed WaitFor[XYZ]-methods to better reflect the timeout. Thanks Judah!</item>
    /// <item>Version 0.80: First version going to CodePlex. Implemented fixes as of mail T.N. 16.2.11.</item>
    /// </list>
    /// <list type="number">
    ///		<listheader>
    ///			<term>Features</term>
    ///			<description>Telnet functionality implemented</description>
    ///		</listheader>
    ///		<item>
    ///			<term>LOGOUT</term>
    ///			<description>Logout functionaliy implemented</description>
    ///		</item>
    ///		<item>
    ///			<term>NAWS</term>
    ///			<description>Sends a window size</description>
    ///		</item>
    ///		<item>
    ///			<term>TERMTYPE</term>
    ///			<description>Sends an "ANSI"-terminal type</description>
    ///		</item>
    ///		<item>
    ///			<term>Other telnet commands</term>
    ///			<description>Will be answered correctly with WILL / WONT</description>
    ///		</item>
    ///		<item>
    ///			<term>ESC-Sequences</term>
    ///			<description>Method dealing with ESC-sequences</description>
    ///		</item>
    ///	</list>
    /// </summary>
    /// <remarks>
    /// The class is NOT thread safe for several connections, so each connection should have its own instance.
    /// </remarks>
    public class Terminal : IDisposable
    {
        #region Fields and properties
        private const byte Cr = 13;
        private const string Endofline = "\r\n"; // CR LF
        private const byte Esc = 27;
        private const String F1 = "\033OP"; // function key
        private const String F10 = "\033[21~";
        private const String F11 = "\033[23~";
        private const String F12 = "\033[24~";
        private const String F2 = "\033OQ";
        private const String F3 = "\033OR";
        private const String F4 = "\033OS";
        private const String F5 = "\033[15~";
        private const String F6 = "\033[17~";
        private const String F7 = "\033[18~";
        private const String F8 = "\033[19~";
        private const String F9 = "\033[20~";
        private const byte Lf = 10;
        private const int ReceiveBufferSize = 10 * 1024; // read a lot
        private const int ScreenXNullcoordinate = 0;
        private const int ScreenYNullCoordinate = 0;
        private const int SendBufferSize = 25; // only small reponses -> only for DOs, WILLs, not for user's responses
        // private const byte TncAo = 245; // F5 The function AO. Abort output
        // private const byte TncAyt = 246; // F6 Are You There The function AYT. 
        // private const byte TncBrk = 243; // F3 Break NVT character BRK.
        // private const byte TncDatamark = 242; // F2 The data stream portion of a Synch. This should always be accompanied by a TCP Urgent notification. 
        private const byte TncDo = 253; // FD Option code: Indicates the request that the other party perform, or confirmation that you are expecting the other party to perform, the indicated option.
        private const byte TncDont = 254; // FE Option code: Indicates the demand that the other party stop performing, or confirmation that you are no longer expecting the other party to perform, the indicated option.
        // private const byte TncEc = 247; // F7 Erase character. The function EC. 
        // private const byte TncEl = 248; // F8 Erase line. The function EL.
        // private const byte TncGa = 249; // F9 Go ahead The GA signal. 
        private const byte TncIac = 255; // FF Data Byte 255
        // private const byte TncIp = 244; // F4 Interrupt Process The function IP. 
        // private const byte TncNop = 241; // No operation
        private const byte TncSb = 250; // FA Option code: Indicates that what follows is subnegotiation of the indicated option.
        private const byte TncSe = 240; // End of subnegotiation parameters
        private const byte TncWill = 251; // FB Option code: Indicates the desire to begin performing, or confirmation that you are now performing, the indicated option.
        private const byte TncWont = 252; // FC Option code: Indicates the refusal to perform, or continue performing, the indicated option.
        private const byte TnoEcho = 1; // 00 echo
        private const byte TnoLogout = 18; // 12 Logout
        private const byte TnoNaws = 31; // 1F Window size
        private const byte TnoNewenv = 39; // 27 New environment option
        private const byte TnoRemoteflow = 33; // 21 Remote flow control
        private const byte TnoTermspeed = 32; // 20 Terminal speed
        private const byte TnoTermtype = 24; // 18 Terminal size
        // private const byte TnoTransbin = 0; // 00 transmit binary
        private const byte TnoXdisplay = 35; // 23 X-Display location
        private const byte TnxIs = 0; // 00 is, e.g. used with SB terminal type
        // private const byte TnxSend = 1; // 01 send, e.g. used with SB terminal type
        private const int Trails = 25; // trails until timeout in "wait"-methods
        /// <summary>The version</summary>
        public const String Version = "0.74";
        private static readonly Regex RegExpCursorLeft = new Regex("\\[\\d*D", RegexOptions.Compiled);
        private static readonly Regex RegExpCursorPosition = new Regex("\\[\\d*;\\d*[Hf]", RegexOptions.Compiled);
        private static readonly Regex RegExpCursorRight = new Regex("\\[\\d*C", RegexOptions.Compiled);
        private static readonly Regex RegExpCursorXPosition = new Regex(";\\d+[Hf]", RegexOptions.Compiled); // column
        private static readonly Regex RegExpCursorYPosition = new Regex("\\[\\d+;", RegexOptions.Compiled); // line
        private static readonly Regex RegExpIp = new Regex(@"\d?\d?\d\.\d?\d?\d\.\d?\d?\d\.\d?\d?\d", RegexOptions.Compiled);
        private static readonly Regex RegExpNumber = new Regex("\\d+", RegexOptions.Compiled);
        private static readonly Regex RegExpScrollingRegion = new Regex("\\[\\d*;\\d*r", RegexOptions.Compiled);
        private readonly string _hostName;
        private readonly int _port;
        private readonly int _timeoutReceive; // timeout in seconds
        private readonly int _timeoutSend; // timeout in seconds for TCP client
        private readonly int _vsHeight;
        private readonly int _vsWidth;
        private byte[] _buffer;
        private AsyncCallback _callBackReceive; // callback method
        private AsyncCallback _callBackSend; // callback method
        private bool _clientInitNaws;
        private bool _firstResponse = true;
        private bool _forceLogout;
        private bool _nawsNegotiated;
        private bool _serverEcho;
        private TcpClient _tcpClient;
        private VirtualScreen _virtualScreen;
        /// <summary>
        /// Property virtual screen
        /// </summary>
        public VirtualScreen VirtualScreen { get { return this._virtualScreen; } }
        /// <summary>
        /// Server echo on?
        /// </summary>
        public bool EchoOn { get { return this._serverEcho; } }
        #endregion Fields / Properties

        #region Constructors and destructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostName">IP address, e.g. 192.168.0.20</param>
        public Terminal(string hostName)
            : this(hostName, 23, 10, 80, 40)
        {
            // nothing further
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="hostName">IP address, e.g. 192.168.0.20</param>
        /// <param name="port">Port, usually 23 for telnet</param>
        /// <param name="timeoutSeconds">Timeout for connections [s], both read and write</param>
        /// <param name="virtualScreenWidth">Screen width for the virtual screen</param>
        /// <param name="virtualScreenHeight">Screen height for the virtual screen</param>
        public Terminal(string hostName, int port, int timeoutSeconds, int virtualScreenWidth, int virtualScreenHeight)
        {
            this._hostName = hostName;
            this._port = port;
            this._timeoutReceive = timeoutSeconds;
            this._timeoutSend = timeoutSeconds;
            this._serverEcho = false;
            this._clientInitNaws = false;
            this._firstResponse = true;
            this._nawsNegotiated = false;
            this._forceLogout = false;
            this._vsHeight = virtualScreenHeight;
            this._vsWidth = virtualScreenWidth;
        }

        /// <summary>
        /// Dispose part, calls Close()
        /// </summary>
        public void Dispose()
        {
            this.Close();
        }

        /// <summary>
        /// Destructor, calls Close()
        /// </summary>
        ~Terminal()
        {
            this.Close();
        }
        #endregion

        #region Connect / Close
        /// <summary>
        /// Connect to the telnet server
        /// </summary>
        /// <returns>true if connection was successful</returns>
        public bool Connect()
        {
            // check for buffer
            if (this._buffer == null)
                this._buffer = new byte[ReceiveBufferSize];

            // virtual screen
            if (this._virtualScreen == null)
                this._virtualScreen = new VirtualScreen(this._vsWidth, this._vsHeight, 1, 1);

            // set the callbacks
            if (this._callBackReceive == null)
                this._callBackReceive = this.ReadFromStream;
            if (this._callBackSend == null)
                this._callBackSend = this.WriteToStream;

            // flags
            this._serverEcho = false;
            this._clientInitNaws = false;
            this._firstResponse = true;
            this._nawsNegotiated = false;
            this._forceLogout = false;

            // return physical connection
            if (this._tcpClient != null)
                return true; // we still have a connection -> ?? better reconnect ??
            try
            {
                // TODO: Improve performance ...?
                // This is not working:	IPAddress ipAddress = IPAddress.Parse(this.hostName);
                //						IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, this.port);
                // because it addresses local endpoints
                this._tcpClient = new TcpClient(this._hostName, this._port) { ReceiveTimeout = this._timeoutReceive, SendTimeout = this._timeoutSend, NoDelay = true };
                this._tcpClient.GetStream().BeginRead(this._buffer, 0, this._buffer.Length, this._callBackReceive, null);
                return true;
            }
            catch
            {
                this._tcpClient = null;
                return false;
            }
        }

        /// <summary>
        /// Closes external resources.
        /// Safe, can be called multiple times
        /// </summary>
        public void Close()
        {
            // physical connection
            if (this._tcpClient != null)
            {
                try
                {
                    // it is important to close the stream
                    // because somehow tcpClient does not physically breaks down
                    // the connection - on "one connection" telnet server the 
                    // server remains blocked if not doing it!
                    this._tcpClient.GetStream().Close();
                    this._tcpClient.Close();
                    this._tcpClient = null;
                }
                catch
                {
                    this._tcpClient = null;
                }
            }

            // clean up
            // fast, "can be done several" times
            this._virtualScreen = null;
            this._buffer = null;
            this._callBackReceive = null;
            this._callBackSend = null;
            this._forceLogout = false;
        }

        // Close()

        /// <summary>
        /// Is connection still open?
        /// </summary>
        /// <returns>true if connection is open</returns>
        public bool IsOpenConnection
        {
            get
            {
                return (this._tcpClient != null);
            }
        }
        #endregion

        #region Send response to Telnet server
        /// <summary>
        /// Send a response to the server
        /// </summary>
        /// <param name="response">response String</param>
        /// <param name="endLine">terminate with appropriate end-of-line chars</param>
        /// <returns>true if sending was OK</returns>
        public bool SendResponse(string response, bool endLine)
        {
            try
            {
                if (!this.IsOpenConnection || this._tcpClient == null) return false;
                if (string.IsNullOrEmpty(response)) return true; // nothing to do
                byte[] sendBuffer = (endLine) ? Encoding.ASCII.GetBytes(response + Endofline) : Encoding.ASCII.GetBytes(response);
                if (sendBuffer.Length < 1) return false;
                this._tcpClient.GetStream().BeginWrite(sendBuffer, 0, sendBuffer.Length, this._callBackSend, null);
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Send function key response to Telnet server
        /// <summary>
        /// Send a Funktion Key response to the server
        /// </summary>
        /// <param name="key">Key number 1-12</param>
        /// <returns>true if sending was OK</returns>
        public bool SendResponseFunctionKey(int key)
        {
            if (key < 1 || key > 12)
                return false;
            switch (key)
            {
                case 1:
                    return this.SendResponse(F1, false);
                case 2:
                    return this.SendResponse(F2, false);
                case 3:
                    return this.SendResponse(F3, false);
                case 4:
                    return this.SendResponse(F4, false);
                case 5:
                    return this.SendResponse(F5, false);
                case 6:
                    return this.SendResponse(F6, false);
                case 7:
                    return this.SendResponse(F7, false);
                case 8:
                    return this.SendResponse(F8, false);
                case 9:
                    return this.SendResponse(F9, false);
                case 10:
                    return this.SendResponse(F10, false);
                case 11:
                    return this.SendResponse(F11, false);
                case 12:
                    return this.SendResponse(F12, false);
                default:
                    // this should never be reached
                    return false;
            }
        }
        #endregion

        #region Send Telnet logout sequence
        /// <summary>
        /// Send a synchronously telnet logout-response
        /// </summary>
        /// <returns></returns>
        public bool SendLogout()
        {
            return this.SendLogout(true);
        }

        /// <summary>
        /// Send a telnet logout-response
        /// </summary>
        /// <param name="synchronous">Send synchronously (true) or asynchronously (false)</param>
        /// <returns></returns>
        public bool SendLogout(bool synchronous)
        {
            byte[] lo = { TncIac, TncDo, TnoLogout };
            try
            {
                if (synchronous)
                {
                    this._tcpClient.GetStream().Write(lo, 0, lo.Length);
                }
                else
                {
                    this._tcpClient.GetStream().BeginWrite(lo, 0, lo.Length, this._callBackSend, null);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // sendLogout
        #endregion

        #region WaitFor-methods
        /// <summary>
        /// Wait for a particular string
        /// </summary>
        /// <param name="searchFor">string to be found</param>
        /// <returns>string found or null if not found</returns>
        public string WaitForString(string searchFor)
        {
            return this.WaitForString(searchFor, false, this._timeoutReceive);
        }

        /// <summary>
        /// Wait for a particular string
        /// </summary>
        /// <param name="searchFor">string to be found</param>
        /// <param name="caseSensitive">case sensitive search</param>
        /// <param name="timeoutSeconds">timeout [s]</param>
        /// <returns>string found or null if not found</returns>
        public string WaitForString(string searchFor, bool caseSensitive, int timeoutSeconds)
        {
            if (this._virtualScreen == null || searchFor == null || searchFor.Length < 1)
                return null;
            // use the appropriate timeout setting, which is the smaller number
            int sleepTimeMs = this.GetWaitSleepTimeMs(timeoutSeconds);
            DateTime endTime = this.TimeoutAbsoluteTime(timeoutSeconds);
            do
            {
                string found;
                lock (this._virtualScreen)
                {
                    found = this._virtualScreen.FindOnScreen(searchFor, caseSensitive);
                }
                if (found != null)
                    return found;
                Thread.Sleep(sleepTimeMs);
            }
            while (DateTime.Now <= endTime);
            return null;
        }

        /// <summary>
        /// Wait for a particular regular expression
        /// </summary>
        /// <param name="regEx">string to be found</param>
        /// <returns>string found or null if not found</returns>
        public string WaitForRegEx(string regEx)
        {
            return this.WaitForRegEx(regEx, this._timeoutReceive);
        }

        /// <summary>
        /// Wait for a particular regular expression
        /// </summary>
        /// <param name="regEx">string to be found</param>
        /// <param name="timeoutSeconds">timeout [s]</param>
        /// <returns>string found or null if not found</returns>
        public string WaitForRegEx(string regEx, int timeoutSeconds)
        {
            if (this._virtualScreen == null || regEx == null || regEx.Length < 1)
                return null;
            int sleepTimeMs = this.GetWaitSleepTimeMs(timeoutSeconds);
            DateTime endTime = this.TimeoutAbsoluteTime(timeoutSeconds);
            do // at least once
            {
                string found;
                lock (this._virtualScreen)
                {
                    found = this._virtualScreen.FindRegExOnScreen(regEx);
                }
                if (found != null)
                    return found;
                Thread.Sleep(sleepTimeMs);
            }
            while (DateTime.Now <= endTime);
            return null;
        }

        /// <summary>
        /// Wait for changed screen. Read further documentation 
        /// on <code>WaitForChangedScreen(int)</code>.
        /// </summary>
        /// <returns>changed screen</returns>
        public bool WaitForChangedScreen()
        {
            return this.WaitForChangedScreen(this._timeoutReceive);
        }

        /// <summary>
        /// Waits for changed screen: This method here resets
        /// the flag of the virtual screen and afterwards waits for
        /// changes.
        /// <p>
        /// This means the method detects changes after the call
        /// of the method, NOT prior.
        /// </p>
        /// <p>
        /// To reset the flag only use <code>WaitForChangedScreen(0)</code>.
        /// </p>
        /// </summary>
        /// <param name="timeoutSeconds">timeout [s]</param>
        /// <remarks>
        /// The property ChangedScreen of the virtual screen is
        /// reset after each call of Hardcopy(). It is also false directly
        /// after the initialization.
        /// </remarks>
        /// <returns>changed screen</returns>
        public bool WaitForChangedScreen(int timeoutSeconds)
        {
            // 1st check
            if (this._virtualScreen == null || timeoutSeconds < 0)
                return false;

            // reset flag: This has been added after the feedback of Mark
            if (this._virtualScreen.ChangedScreen)
                this._virtualScreen.Hardcopy(false);

            // Only reset
            if (timeoutSeconds <= 0)
                return false;

            // wait for changes, the goal is to test at TRAILS times, if not timing out before
            int sleepTimeMs = this.GetWaitSleepTimeMs(timeoutSeconds);
            DateTime endTime = this.TimeoutAbsoluteTime(timeoutSeconds);
            do // run at least once
            {
                lock (this._virtualScreen)
                {
                    if (this._virtualScreen.ChangedScreen)
                        return true;
                }
                Thread.Sleep(sleepTimeMs);
            }
            while (DateTime.Now <= endTime);
            return false;
        }

        // WaitForChangedScreen

        /// <summary>
        /// Wait (=Sleep) for n seconds
        /// </summary>
        /// <param name="seconds">seconds to sleep</param>
        public void Wait(int seconds)
        {
            if (seconds > 0)
                Thread.Sleep(seconds * 1000);
        }

        // Wait

        /// <summary>
        /// Helper method: 
        /// Get the appropriate timeout, which is the bigger number of
        /// timeoutSeconds and this.timeoutReceive (TCP client timeout)
        /// </summary>
        /// <param name="timeoutSeconds">timeout in seconds</param>
        private int GetWaitTimeout(int timeoutSeconds)
        {
            if (timeoutSeconds < 0 && this._timeoutReceive < 0) return 0;
            if (timeoutSeconds < 0) return this._timeoutReceive; // no valid timeout, return other one
            return (timeoutSeconds >= this._timeoutReceive) ? timeoutSeconds : this._timeoutReceive;
        }

        /// <summary>
        /// Helper method: 
        /// Get the appropriate sleep time based on timeout and TRIAL
        /// </summary>
        /// <param name="timeoutSeconds">timeout ins seconds</param>
        private int GetWaitSleepTimeMs(int timeoutSeconds)
        {
            return (this.GetWaitTimeout(timeoutSeconds) * 1000) / Trails;
        }

        /// <summary>
        /// Helper method: 
        /// Get the end time, which is "NOW" + timeout
        /// </summary>
        /// <param name="timeoutSeconds">timeout int seconds</param>
        private DateTime TimeoutAbsoluteTime(int timeoutSeconds)
        {
            return DateTime.Now.AddSeconds(this.GetWaitTimeout(timeoutSeconds));
        }
        #endregion

        #region Callback function ReadFromStream
        /// <summary>
        /// Callback function to read from the network stream
        /// </summary>
        /// <param name="asyncResult">Callback result</param>
        private void ReadFromStream(IAsyncResult asyncResult)
        {
            if (asyncResult == null || this._tcpClient == null)
            {
                this.Close();
                return;
            }

            // read
            try
            {
                // bytes read
                // NOT needed: this.callBackReceive.EndInvoke(asyncResult); -> exception
                int bytesRead = this._tcpClient.GetStream().EndRead(asyncResult);

                if (bytesRead > 0)
                {
                    // Translate the data and write output to Virtual Screen
                    // DO this thread save to make sure we do not "READ" from screen meanwhile
                    lock (this._virtualScreen)
                    {
                        this.ParseAndRespondServerStream(bytesRead);
                    }

                    // Reinitialize callback
                    this.CleanBuffer(bytesRead);
                    if (this._forceLogout)
                        this.Close();
                    else
                        this._tcpClient.GetStream().BeginRead(this._buffer, 0, this._buffer.Length, this._callBackReceive, null);
                }
                else
                    // the connection was terminated by the server
                    this.Close();
            }
            catch
            {
                // the connection was terminated by the server
                this.Close();
            }
        }
        #endregion

        #region Callback function: Write to stream
        /// <summary>
        /// Callback function to write to the network stream
        /// </summary>
        /// <param name="asyncResult">Callback result</param>
        private void WriteToStream(IAsyncResult asyncResult)
        {
            if (asyncResult == null || this._tcpClient == null)
            {
                this.Close();
                return;
            }

            // write 
            try
            {
                this._tcpClient.GetStream().EndWrite(asyncResult);
            }
            catch
            {
                this.Close(); // the connection was terminated by the server
            }
        }

        // write network stream
        #endregion

        #region ParseAndRespondServerStream
        /// <summary>
        /// Go thru the data received and answer all technical server
        /// requests (TELNET negotiations).
        /// </summary>
        /// <param name="bytesRead">number of bytes read</param>
        /// <remarks>
        /// Thread saftey regarding the virtual screen needs to be considered
        /// </remarks>
        private void ParseAndRespondServerStream(int bytesRead)
        {
            // reponse to server
            var response = new MemoryStream(SendBufferSize); // answer usually will be small: "a few bytes only"

            // cycle thru the buffer
            int bc = 0;
            while (this._buffer != null && bc < bytesRead && bc < this._buffer.Length)
            {
                try
                {
                    switch (this._buffer[bc])
                    {
                        // ESC
                        case Esc:
                            bc = this.ParseEscSequence(bc, response);
                            break;
                        case Cr:
                            this._virtualScreen.WriteByte(Cr);
                            break;
                        case Lf:
                            this._virtualScreen.WriteByte(Lf);
                            break;
                        // DO
                        case TncIac:
                            bc++;
                            switch (this._buffer[bc])
                            {
                                case TncDo:
                                    bc++;
                                    switch (this._buffer[bc])
                                    {
                                        // DO ...
                                        case TnoTermspeed:
                                            TelnetWont(TnoTermspeed, response); // no negotiation about speed
                                            break;
                                        case TnoNaws:
                                            if (!this._clientInitNaws)
                                                TelnetWill(TnoNaws, response); // negotiation about window size
                                            TelnetSubNaws(this._virtualScreen.Width, this._virtualScreen.Height, response);
                                            this._nawsNegotiated = true;
                                            break;
                                        case TnoTermtype:
                                            TelnetWill(TnoTermtype, response); // negotiation about terminal type
                                            break;
                                        case TnoXdisplay:
                                            TelnetWont(TnoXdisplay, response); // no negotiation about X-Display
                                            break;
                                        case TnoNewenv:
                                            TelnetWont(TnoNewenv, response); // no environment
                                            break;
                                        case TnoEcho:
                                            TelnetWont(TnoEcho, response); // no echo from client
                                            break;
                                        case TnoRemoteflow:
                                            TelnetWont(TnoRemoteflow, response); // no echo from client
                                            break;
                                        case TnoLogout:
                                            TelnetWill(TnoLogout, response); // no echo from client
                                            this._forceLogout = true;
                                            break;
                                        default:
                                            // all we do not understand =>
                                            // WONT
                                            TelnetWont(this._buffer[bc], response); // whatever -> WONT
                                            break;
                                    } // SWITCH DO XX
                                    break;
                                // DONT
                                case TncDont:
                                    bc++; // ignore DONTs
                                    break;
                                // SUB-NEGOTIATION
                                case TncSb:
                                    bc++;
                                    switch (this._buffer[bc])
                                    {
                                        // SUB ...
                                        case TnoTermtype:
                                            bc++; // the follwing byte is usually a send-request ("SEND"), just read the byte 
                                            TelnetSubIsAnsi(response);
                                            break;
                                        case TnoNaws:
                                            bc++; // the follwing byte is usually a send-request ("SEND"), just read the byte 
                                            TelnetSubNaws(this._virtualScreen.Width, this._virtualScreen.Height, response);
                                            this._nawsNegotiated = true;
                                            break;
                                        default:
                                            // read until the end of the subrequest
                                            while (this._buffer[bc] != TncSe && bc < this._buffer.Length)
                                            {
                                                bc++;
                                            }
                                            break;
                                    } // SUB NEG XX
                                    break;
                                // WILL AND WONTs FROM SERVER
                                case TncWill:
                                    // Server's WILLs
                                    bc++;
                                    switch (this._buffer[bc])
                                    {
                                        case TnoEcho:
                                            this._serverEcho = true;
                                            TelnetDo(this._buffer[bc], response);
                                            break;
                                        case TnoLogout:
                                            // usually a reponse on my logout
                                            // I do no say anything but force the end
                                            this._forceLogout = true;
                                            break;
                                        default:
                                            // other WILLs OF SERVER -> confirm
                                            TelnetDo(this._buffer[bc], response);
                                            break;
                                    }
                                    break;
                                case TncWont:
                                    bc++;
                                    // Server's WONTs
                                    bc++;
                                    switch (this._buffer[bc])
                                    {
                                        case TnoEcho:
                                            this._serverEcho = false;
                                            break;
                                    }
                                    break;
                            }
                            break;
                        default:
                            // no command, data
                            this._virtualScreen.WriteByte(this._buffer[bc]);
                            break;
                    } // switch
                    bc++;
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }
            } // while

            //
            // send the response
            //
            if (this._firstResponse)
            {
                // send some own WILLs even if not asked as DOs
                if (!this._nawsNegotiated)
                {
                    TelnetWill(TnoNaws, response);
                    this._clientInitNaws = true;
                }
            } // 1st response

            //
            // respond synchronously !
            // -> we know that we really have send the bytes
            //
            if (response.Position > 0)
            {
                byte[] r = MemoryStreamToByte(response);
                if (r != null && r.Length > 0)
                {
                    this._tcpClient.GetStream().Write(r, 0, r.Length);
                    this._tcpClient.GetStream().Flush();
                    this._firstResponse = false;
                }
            }

            // clean up
            try
            {
                response.Close();
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch
            {
                // ignore
            }
            // ReSharper restore EmptyGeneralCatchClause
        }

        // method
        #endregion

        #region ParseEscSequence
        /// <summary>
        /// Deal with ESC Sequences as in VT100, ..
        /// </summary>
        /// <param name="bc">current buffer counter</param>
        /// <param name="response">Stream for the response (back to Telnet server)</param>
        /// <returns>new buffer counter (last byte dealed with)</returns>
        /// <remarks>
        /// Thread saftey regarding the virtual screen needs to be considered
        /// </remarks>
        private int ParseEscSequence(int bc, MemoryStream response)
        {
            // some sequences can only be terminated by the end characters
            // (they contain wildcards) => 
            // a switch / case decision is not feasible
            if (this._buffer == null) return bc;

            // find the byte after ESC
            if (this._buffer[bc] == Esc) bc++;

            // now handle sequences
            int m;

            // Cursor Movement Commands 
            //  Index                           ESC D
            //  Next Line                       ESC E
            //  Reverse index                   ESC M
            //  Save cursor and attributes      ESC 7
            //  Restore cursor and attributes   ESC 8
            if ((m = this.MatchSequence(bc, "D")) > -1) return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "E")) > -1)
            {
                this._virtualScreen.CursorNextLine();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "M")) > -1) return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "7")) > -1) return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "8")) > -1) return (bc + m - 1);

            // Cursor movements
            //  Cursor forward (right)          ESC [ Pn C, e.g. "[12C"	
            //  Cursor backward (left)          ESC [ Pn D,	e.g. "[33D"
            //	Direct cursor addressing        ESC [ Pl; Pc H  or
            //									ESC [ Pl; Pc f
            // Reg Exp: \[ = [  \d = 0-9  + 1 time or more
            if ((m = this.MatchRegExp(bc, RegExpCursorLeft)) > -1)
            {
                this._virtualScreen.MoveCursor(-1);
                return (bc + m - 1);
            }
            if ((m = this.MatchRegExp(bc, RegExpCursorRight)) > -1)
            {
                this._virtualScreen.MoveCursor(1);
                return (bc + m - 1);
            }
            if ((m = this.MatchRegExp(bc, RegExpCursorPosition)) > -1)
            {
                string sequence = Encoding.ASCII.GetString(this._buffer, bc, m);
                int nx = NewCursorXPosition(sequence);
                int ny = NewCursorYPosition(sequence);
                this._virtualScreen.MoveCursorTo(nx, ny);
                return (bc + m - 1);
            }
            // Scrolling region 
            //  ESC [ Pt ; Pb r
            if ((m = this.MatchRegExp(bc, RegExpScrollingRegion)) > -1)
            {
                return (bc + m - 1);
            }
            // Line Size (Double-Height and Double-Width) Commands
            //  Change this line to double-height top half      ESC # 3
            //  Change this line to double-height bottom half   ESC # 4
            //  Change this line to single-width single-height  ESC # 5
            //  Change this line to double-width single-height  ESC # 6
            if ((m = this.MatchSequence(bc, "#3")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "#4")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "#5")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "#6")) > -1)
                return (bc + m - 1);
            // Erasing
            //  From cursor to end of line              ESC [ K  or ESC [ 0 K
            //  From beginning of line to cursor        ESC [ 1 K
            //  Entire line containing cursor           ESC [ 2 K
            //  From cursor to end of screen            ESC [ J  or ESC [ 0 J
            //  From beginning of screen to cursor      ESC [ 1 J
            //  Entire screen                           ESC [ 2 J
            if ((m = this.MatchSequence(bc, "[K")) > -1)
            {
                this._virtualScreen.CleanLine(this._virtualScreen.CursorX, this._virtualScreen.CursorXRight);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[0K")) > -1)
            {
                this._virtualScreen.CleanLine(this._virtualScreen.CursorX, this._virtualScreen.CursorXRight);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[1K")) > -1)
            {
                this._virtualScreen.CleanLine(this._virtualScreen.CursorXLeft, this._virtualScreen.CursorX);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[2K")) > -1)
            {
                this._virtualScreen.CleanLine(this._virtualScreen.CursorXLeft, this._virtualScreen.CursorXRight);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[J")) > -1)
            {
                this._virtualScreen.CleanFromCursor();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[0J")) > -1)
            {
                this._virtualScreen.CleanFromCursor();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[1J")) > -1)
            {
                this._virtualScreen.CleanToCursor();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[2J")) > -1)
            {
                // erase entire screen
                this._virtualScreen.CleanScreen();
                return (bc + m - 1);
            }
            // Hardcopy                ESC # 7
            // Graphic processor ON    ESC 1
            // Graphic processor OFF   ESC 2
            if ((m = this.MatchSequence(bc, "#7")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "1")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "2")) > -1)

                return (bc + m - 1);

            // TAB stops
            //  Set tab at current column               ESC H
            //  Clear tab at curent column              ESC [ g or ESC [ 0 g
            //  Clear all tabs                          ESC [ 3 g
            if ((m = this.MatchSequence(bc, "H")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[g")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[0g")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[3g")) > -1)
                return (bc + m - 1);

            // Modes:
            //  Line feed/new line   New line    ESC [20h   Line feed   ESC [20l
            //  Cursor key mode      Application ESC [?1h   Cursor      ESC [?1l
            //  ANSI/VT52 mode       ANSI        N/A        VT52        ESC [?2l
            //  Column mode          132 Col     ESC [?3h   80 Col      ESC [?3l
            //  Scrolling mode       Smooth      ESC [?4h   Jump        ESC [?4l
            //  Screen mode          Reverse     ESC [?5h   Normal      ESC [?5l
            //  Origin mode          Relative    ESC [?6h   Absolute    ESC [?6l
            //  Wraparound           On          ESC [?7h   Off         ESC [?7l
            //  Auto repeat          On          ESC [?8h   Off         ESC [?8l
            //  Interlace            On          ESC [?9h   Off         ESC [?9l
            //  Graphic proc. option On          ESC 1      Off         ESC 2
            //  Keypad mode          Application ESC =      Numeric     ESC >
            if ((m = this.MatchSequence(bc, "[20h")) > -1)
            {
                this._virtualScreen.CursorNextLine();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[20l")) > -1)
            {
                response.WriteByte(10);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "[?1h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?1l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?3h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?3l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?4h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?4l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?5h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?5l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?6h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?6l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?7h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?7l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?8h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?8l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?9h")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "[?9l")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "1")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "2")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "=")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, ">")) > -1)
                return (bc + m - 1);

            // VT 52 compatibility mode
            //  Cursor Up                               ESC A
            //  Cursor Down                             ESC B
            //  Cursor Right                            ESC C
            //  Cursor Left                             ESC D
            //  Select Special Graphics character set   ESC F
            //  Select ASCII character set              ESC G
            //  Cursor to home                          ESC H
            //  Reverse line feed                       ESC I
            //  Erase to end of screen                  ESC J
            //  Erase to end of line                    ESC K
            //  Direct cursor address                   ESC Ylc         (see note 1)
            //  Identify                                ESC Z           (see note 2)
            //  Enter alternate keypad mode             ESC =
            //  Exit alternate keypad mode              ESC >
            //  Enter ANSI mode                         ESC <
            if ((m = this.MatchSequence(bc, "A")) > -1)
            {
                this._virtualScreen.MoveCursorVertical(-1);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "B")) > -1)
            {
                this._virtualScreen.MoveCursorVertical(1);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "C")) > -1)
            {
                this._virtualScreen.MoveCursorVertical(1);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "D")) > -1)
            {
                this._virtualScreen.MoveCursorVertical(-1);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "F")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "G")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "H")) > -1)
            {
                this._virtualScreen.CursorReset();
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "I")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "J")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "K")) > -1)
            {
                this._virtualScreen.CleanLine(this._virtualScreen.CursorX, this._virtualScreen.CursorXRight);
                return (bc + m - 1);
            }
            if ((m = this.MatchSequence(bc, "Ylc")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "Z")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "=")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, "<")) > -1)
                return (bc + m - 1);
            if ((m = this.MatchSequence(bc, ">")) > -1)
                return (bc + m - 1);
            // nothing found
            return bc;
        }
        #endregion ParseEscSequence

        #region MatchSequence- and MatchRegEx-methods
        /// <summary>
        /// Does the sequence match the buffer starting at 
        /// current index?
        /// </summary>
        /// <param name="bufferCounter">Current buffer counter</param>
        /// <param name="sequence">Bytes need to match</param>
        /// <param name="ignoreIndex">Index of bytes which do not NEED to match, e.g. 2 means the 3rd byte (index 2) does not need to match</param>
        /// <returns>Number of characters matching</returns>
        private int MatchSequence(int bufferCounter, byte[] sequence, int[] ignoreIndex = null)
        {
            if (sequence == null || this._buffer == null)
                return -1;
            if (this._buffer.Length < (bufferCounter + sequence.Length))
                return -1; // overflow
            for (int i = 0; i < sequence.Length; i++)
            {
                if (this._buffer[bufferCounter + i] != sequence[i])
                {
                    // not a match
                    if (ignoreIndex == null || ignoreIndex.Length < 1)
                        return -1; // no wildcards
                    bool wildcard = false;
                    // ReSharper disable LoopCanBeConvertedToQuery
                    foreach (int t in ignoreIndex)
                    {
                        if (t == i)
                        {
                            wildcard = true;
                            break;
                        }
                    }
                    // ReSharper restore LoopCanBeConvertedToQuery

                    if (!wildcard)
                        return -1; // no wildcard found
                } // no match
            }
            return sequence.Length;
        }

        /// <summary>
        /// Does the sequence match the buffer?
        /// </summary>
        /// <param name="bufferCounter">Current buffer counter</param>
        /// <param name="sequence">String needs to match</param>
        /// <returns>Number of characters matching</returns>
        private int MatchSequence(int bufferCounter, string sequence)
        {
            if (sequence == null)
                return -1;
            return this.MatchSequence(bufferCounter, Encoding.ASCII.GetBytes(sequence));
        }

        /// <summary>
        /// Match a regular Expression
        /// </summary>
        /// <param name="bufferCounter">Current buffer counter</param>
        /// <param name="r">Regular expression object</param>
        /// <returns>Number of characters matching</returns>
        private int MatchRegExp(int bufferCounter, Regex r)
        {
            if (r == null || this._buffer == null || this._buffer.Length < bufferCounter)
                return -1;
            // build a dummy string
            // which can be checked
            const int dsl = 10;
            string dummyString = this._buffer.Length >= (bufferCounter + dsl) ? Encoding.ASCII.GetString(this._buffer, bufferCounter, dsl) : Encoding.ASCII.GetString(this._buffer, bufferCounter, this._buffer.Length - bufferCounter);
            if (String.IsNullOrEmpty(dummyString)) return -1;
            Match m = r.Match(dummyString);
            if (m.Success && m.Index == 0) return m.Length;
            return -1;
        }
        #endregion

        #region Cursor movements in virtual screen
        /// <summary>
        /// Find the X position in a VT cursor position sequence.
        /// This only works if the sequence is a valid position sequence!
        /// </summary>
        /// <param name="escSequence">Valid position sequence</param>
        /// <returns>X position (column)</returns>
        private static int NewCursorXPosition(string escSequence)
        {
            const int DEFAULT = ScreenXNullcoordinate;
            if (escSequence == null)
                return -1; // error
            Match m = RegExpCursorXPosition.Match(escSequence);
            if (!m.Success) return DEFAULT; // default;
            m = RegExpNumber.Match(m.Value);
            if (m.Success)
            {
                try
                {
                    return int.Parse(m.Value);
                }
                catch
                {
                    return DEFAULT;
                }
            }
            return DEFAULT;
        }

        // method

        /// <summary>
        /// Find the Y position in a VT cursor position sequence.
        /// This only works if the sequence is a valid position sequence!
        /// </summary>
        /// <param name="escSequence">Valid position sequence</param>
        /// <returns>Y position (column)</returns>
        private static int NewCursorYPosition(string escSequence)
        {
            const int DEFAULT = ScreenYNullCoordinate;
            if (escSequence == null)
                return -1; // error
            Match m = RegExpCursorYPosition.Match(escSequence);
            if (!m.Success) return DEFAULT; // default;
            m = RegExpNumber.Match(m.Value);
            if (m.Success)
            {
                try
                {
                    return int.Parse(m.Value);
                }
                catch
                {
                    return DEFAULT;
                }
            }
            return DEFAULT;
        }
        #endregion

        #region Telnet sub-responses as WILL, WONT ..
        /// <summary>
        /// Add a "WILL" response, e.g. "WILL negotiate about terminal size"
        /// </summary>
        /// <param name="willDoWhat"></param>
        /// <param name="response"></param>
        private static void TelnetWill(byte willDoWhat, MemoryStream response)
        {
            response.WriteByte(TncIac);
            response.WriteByte(TncWill);
            response.WriteByte(willDoWhat);
        }

        /// <summary>
        /// Add a "WONT" response, e.g. "WONT negotiate about terminal size"
        /// </summary>
        /// <param name="wontDoWhat"></param>
        /// <param name="response"></param>
        private static void TelnetWont(byte wontDoWhat, MemoryStream response)
        {
            response.WriteByte(TncIac);
            response.WriteByte(TncWont);
            response.WriteByte(wontDoWhat);
        }

        /// <summary>
        /// Add a "DO" response, e.g. "DO ..."
        /// </summary>
        /// <param name="doWhat"></param>
        /// <param name="response"></param>
        private static void TelnetDo(byte doWhat, MemoryStream response)
        {
            response.WriteByte(TncIac);
            response.WriteByte(TncDo);
            response.WriteByte(doWhat);
        }

        /*
        /// <summary>
        /// Add a "DONT" response, e.g. "DONT ..."
        /// </summary>
        /// <param name="dontDoWhat"></param>
        /// <param name="response"></param>
        private static void TelnetDont(byte dontDoWhat, MemoryStream response)
        {
            response.WriteByte(TncIac);
            response.WriteByte(TncDont);
            response.WriteByte(dontDoWhat);
        }
        */

        /// <summary>
        /// Add a telnet sub-negotiation for ANSI 
        /// terminal
        /// </summary>
        /// <param name="response">MemoryStream</param>
        private static void TelnetSubIsAnsi(MemoryStream response)
        {
            response.WriteByte(TncIac);
            response.WriteByte(TncSb);
            response.WriteByte(TnoTermtype);
            response.WriteByte(TnxIs);
            response.WriteByte(65); // "A"
            response.WriteByte(78); // "N"
            response.WriteByte(83); // "S"
            response.WriteByte(73); // "I"
            response.WriteByte(TncIac);
            response.WriteByte(TncSe);
        }

        // method

        /// <summary>
        /// Telnet sub send terminal size.
        /// </summary>
        /// <param name="w">window width</param>
        /// <param name="h">window height</param>
        /// <param name="response">MemoryStream</param>
        private static void TelnetSubNaws(int w, int h, MemoryStream response)
        {
            var wl = (byte)(0x00FF & w);
            var wh = (byte)(0xFF00 & w);
            var hl = (byte)(0x00FF & h);
            var hh = (byte)(0xFF00 & h);
            response.WriteByte(TncIac);
            response.WriteByte(TncSb);
            response.WriteByte(TnoNaws);
            response.WriteByte(wh);
            response.WriteByte(wl);
            response.WriteByte(hh);
            response.WriteByte(hl);
            response.WriteByte(TncIac);
            response.WriteByte(TncSe);
        }

        // method
        #endregion

        #region Misc. helper-methods
        /// <summary>
        /// Cleans the buffer - not necessary since the values
        /// would just be overwritten - but useful for debugging!
        /// </summary>
        /// <param name="bytesRead">Bytes read and need cleaning</param>
        private void CleanBuffer(int bytesRead)
        {
            if (this._buffer == null) return;
            for (int i = 0; i < bytesRead && i < this._buffer.Length; i++)
            {
                this._buffer[i] = 0;
            }
        }

        // method

        /// <summary>
        /// The MemoryStream bas a bigger byte buffer than bytes
        /// were really written to it. This method fetches all bytes
        /// up the the position written to.
        /// </summary>
        /// <param name="ms">MemoryStream</param>
        /// <returns>really written bytes</returns>
        private static byte[] MemoryStreamToByte(MemoryStream ms)
        {
            // I've tried several options to convert this
            // This one here works but may be improved.
            // ms.Read(wb, 0, wb.Length); did not work
            // ms.ToArray delivers the whole buffer not only the written bytes
            if (ms == null) return null;
            if (ms.Position < 2) return new byte[0];

            // convert
            var wb = new byte[ms.Position];
            byte[] allBytes = ms.ToArray();
            for (int i = 0; i < wb.Length && i < allBytes.Length; i++)
            {
                wb[i] = allBytes[i];
            }
            return wb;
        }

        // method

        /// <summary>
        /// Helper to find a valid IP with a string
        /// </summary>
        /// <param name="candidate">search this string for IP</param>
        /// <returns>IP address or null</returns>
        public static string FindIpAddress(string candidate)
        {
            if (candidate == null) return null;
            Match m = RegExpIp.Match(candidate);
            return m.Success ? m.Value : null;
        }
        #endregion
    } // class
    #region Custom exceptions
    /// <summary>
    /// Exception dealing with connectivity
    /// </summary>
    public class TelnetException : ApplicationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception's message</param>
        public TelnetException(string message)
            : base(message)
        {
            // further code
        }
    } // Exception class

    /// <summary>
    /// Exception dealing with parsing ...
    /// </summary>
    public class TerminalException : ApplicationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Exception's message</param>
        public TerminalException(string message)
            : base(message)
        {
            // further code
        }
    } // Exception class
    #endregion
    /// <summary>
    /// Implements a simple "screen". This is used by telnet.
    /// <p>
    /// The x (rows) and y (columns) values may have an offset. If the offset
    /// is 0/0 the left upper corner is [0,0], or 0-based. With an offset of 1/1
    /// the left upper corner is [1,1], or 1-based.
    /// </p>
    /// </summary>
    /// <remarks>
    /// The class is not thread safe (e.g. search in buffer and modification
    /// of buffer must not happen. It is duty of the calling class to guarantee this.
    /// </remarks>
    public class VirtualScreen : IDisposable
    {
        /// <summary>
        /// ASCII code for Space
        /// </summary>
        public const byte Space = 32;

        // Width

        // External cursor values allowing an offset and thus
        // 0-based or 1-based coordinates
        private readonly int _offsetx;
        private readonly int _offsety;
        private bool _changedScreen;
        private int _cursorx0;
        private int _cursory0;
        private string _screenString;
        private string _screenStringLower;
        private int _visibleAreaY0Bottom;
        private int _visibleAreaY0Top;
        private byte[,] _vs;

        /// <summary>
        /// Constructor (offset 0/0)
        /// </summary>
        /// <param name="width">Screen's width</param>
        /// <param name="height">Screen's height</param>
        public VirtualScreen(int width, int height)
            : this(width, height, 0, 0)
        {
            // nothing here
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">Screen's width</param>
        /// <param name="height">Screen's height</param>
        /// <param name="xOffset">Screen coordinates are 0,1 .. based</param>
        /// <param name="yOffset">Screen coordinates are 0,1 .. based</param>
        public VirtualScreen(int width, int height, int xOffset, int yOffset)
        {
            this._offsetx = xOffset;
            this._offsety = yOffset;
            this._vs = new byte[width, height];
            this.CleanScreen();
            this._changedScreen = false; // reset becuase constructor
            this._visibleAreaY0Top = 0;
            this._visibleAreaY0Bottom = height - 1;
            this.CursorReset();
        }

        /// <summary>
        /// window size 
        /// </summary>
        public int Width { get { return this._vs == null ? 0 : this._vs.GetLength(0); } } // Width

        /// <summary>
        /// Window height
        /// </summary>
        public int Height
        {
            get
            {
                if (this._vs == null) return 0;
                return this._vs.GetLength(1);
            }
        }

        /// <summary>
        /// Cursor position with offset considered
        /// </summary>
        public int CursorX
        {
            get
            {
                // 2004-09-01 fixed to plus based on mail of Steve
                return this.CursorX0 + this._offsetx;
            }
            set { this.CursorX0 = value - this._offsetx; }
        } // X

        /// <summary>
        /// Cursor position with offset considered
        /// </summary>
        public int CursorY
        {
            get
            {
                // 2004-09-01 fixed to plus based on mail of Steve
                return this.CursorY0 + this._offsety;
            }
            set { this.CursorY0 = value - this._offsety; }
        } // Y

        /// <summary>
        /// X Offset 
        /// </summary>
        public int CursorXLeft { get { return this._offsetx; } }

        /// <summary>
        /// X max value 
        /// </summary>
        public int CursorXRight { get { return this.Width - 1 + this._offsetx; } }
        /// <summary>
        /// Y max value 
        /// </summary>
        public int CursorYMax { get { return this.Height - 1 + this._offsety; } }
        /// <summary>
        /// Y max value 
        /// </summary>
        public int CursorYMin { get { return this._offsety; } }

        /// <summary>
        /// 0-based coordinates for cursor, internally used.
        /// </summary>
        private int CursorX0
        {
            get { return this._cursorx0; }
            set
            {
                if (value <= 0)
                    this._cursorx0 = 0;
                else if (value >= this.Width)
                    this._cursorx0 = this.Width - 1;
                else
                    this._cursorx0 = value;
            }
        }
        /// <summary>
        /// 0-based coordinates for cursor, internally used
        /// </summary>
        private int CursorY0 { get { return this._cursory0; } set { this._cursory0 = value <= 0 ? 0 : value; } }

        // screen array [x,y]

        /// <summary>
        /// Changed screen buffer ?
        /// </summary>
        public bool ChangedScreen { get { return this._changedScreen; } }

        #region IDisposable Members
        /// <summary>
        /// Clean everything up
        /// </summary>
        public void Dispose()
        {
            this._vs = null; // break link to array
            this._screenString = null;
            this._screenStringLower = null;
        }
        #endregion

        /// <summary>
        /// Reset the cursor to upper left corner
        /// </summary>
        public void CursorReset()
        {
            this.CursorY0 = 0;
            this.CursorX0 = 0;
        }

        /// <summary>
        /// Move the cursor to the beginning of the next line
        /// </summary>
        public void CursorNextLine()
        {
            this.Write("\n\r");
        }

        /// <summary>
        /// Set the cursor (offset coordinates)
        /// </summary>
        /// <param name="x">X Position (lines) with offset considered</param>
        /// <param name="y">Y Position (columns) with offset considered</param>
        /// <remarks>
        /// Use the method MoveCursorTo(x,y) when upscrolling should
        /// be supported
        /// </remarks>
        public void CursorPosition(int x, int y)
        {
            this.CursorX = x;
            this.CursorY = y;
        }

        /// <summary>
        /// Clean the screen and reset cursor.
        /// </summary>
        /// <remarks>
        /// Changes the output-flag and scrolledUp attribute!
        /// </remarks>
        public void CleanScreen()
        {
            int lx = this._vs.GetLength(0);
            int ly = this._vs.GetLength(1);
            for (int y = 0; y < ly; y++)
            {
                for (int x = 0; x < lx; x++)
                {
                    this._vs[x, y] = Space;
                }
            }
            this.CursorReset(); // cursor back to beginning
            this._changedScreen = true;
            this._visibleAreaY0Top = 0;
            this._visibleAreaY0Bottom = this.Height - 1;
        }

        // cleanScreen

        /// <summary>
        /// Cleans a screen area, all values are
        /// considering any offset
        /// </summary>
        /// <remarks>
        /// - Changes the output-flag!
        /// - Visible area is considered
        /// </remarks>
        /// <param name="xstart">upper left corner (included)</param>
        /// <param name="ystart">upper left corner (included)</param>
        /// <param name="xend">lower right corner (included)</param>
        /// <param name="yend">lower right corner (included)</param>
        public void CleanScreen(int xstart, int ystart, int xend, int yend)
        {
            if (this._vs == null || xend <= xstart || yend <= ystart || xstart < this._offsetx || xend < this._offsetx || ystart < this._offsety || yend < this._offsety)
                return; // nothing to do

            int x0Start = xstart - this._offsetx;
            int y0Start = ystart - this._offsety - this._visibleAreaY0Top;
            if (y0Start < 0) y0Start = 0; // only visible area
            int x0End = xend - this._offsetx;
            int y0End = yend - this._offsety - this._visibleAreaY0Top;
            if (y0End < 0) return; // nothing to do

            int lx = this._vs.GetLength(0);
            int ly = this._vs.GetLength(1);

            if (x0End >= lx) x0End = lx - 1;
            if (y0End >= ly) y0End = ly - 1;

            for (int y = y0Start; y <= y0End; y++)
            {
                for (int x = x0Start; x <= x0End; x++)
                {
                    this._vs[x, y] = Space;
                }
            }
            this._changedScreen = true;
        }

        /// <summary>
        /// Clean the current line. Changes the output-flag! Visible area is considered.
        /// </summary>
        /// <param name="xStart">X with offset considered</param>
        /// <param name="xEnd">X with offset considered</param>
        public void CleanLine(int xStart, int xEnd)
        {
            int x0S = xStart - this._offsetx;
            int x0E = xEnd - this._offsetx;

            if (xStart < xEnd) return;
            if (x0S < 0) x0S = 0;
            if (x0E >= this.Width) x0E = this.Width - 1;

            int y = this._cursory0 - this._visibleAreaY0Top;
            if (this._vs == null || y < 0 || y > this._vs.GetLength(1)) return;

            for (int x = x0S; x <= x0E; x++)
            {
                this._vs[x, y] = Space;
            }
            this._changedScreen = true;
        }

        /// <summary>
        /// Clean screen including the cursor position.
        /// Changes the output-flag! The visible area is considered.
        /// </summary>
        public void CleanToCursor()
        {
            int y = this.CursorY - 1; // line before
            if (y >= this._offsety)
                this.CleanScreen(this.CursorXLeft, this._offsety, this.CursorXRight, y);
            this.CleanLine(this.CursorXLeft, this.CursorX);
            this._changedScreen = true;
        }

        /// <summary>
        /// Clean screen including the cursor position.
        /// Changes the output-flag! The Visible area is considered.
        /// </summary>
        public void CleanFromCursor()
        {
            int y = this.CursorY; // line before FIX: changed from this.CursorY + 1; T.Neumann 160211 (2)
            if (y <= this._visibleAreaY0Bottom + this._offsety)
                this.CleanScreen(this.CursorXLeft, y, this.CursorXRight, this._visibleAreaY0Bottom + this._offsety);
            this.CleanLine(this.CursorX, this.CursorXRight);
            this._changedScreen = true;
        }

        /// <summary>
        /// Scrolls up about n lines.Changes the output-flag!
        /// </summary>
        /// <param name="lines"></param>
        /// TODO: Do we have to change the coordinates offset?
        /// TODO: Is line 5 after 2 lines scrolling now line 3 or still 5?
        /// <returns>number of lines scrolled</returns>
        public int ScrollUp(int lines)
        {
            // scrolls up about n lines
            if (lines < 1) return 0;

            int lx = this._vs.GetLength(0);
            int ly = this._vs.GetLength(1);

            if (lines >= ly)
            {
                // we need to save the visible are info
                int vat = this._visibleAreaY0Top;
                int vab = this._visibleAreaY0Bottom;
                this.CleanScreen();
                this._visibleAreaY0Top = vat;
                this._visibleAreaY0Bottom = vab;
            }
            else
            {
                for (int y = lines; y < ly; y++)
                {
                    int yTo = y - lines;
                    for (int x = 0; x < lx; x++)
                    {
                        this._vs[x, yTo] = this._vs[x, y];
                    }
                } // for copy over
                // delete the rest
                this.CleanScreen(this._offsetx, ly - lines, lx + this._offsetx, ly - 1 + this._offsety);
            }
            this._changedScreen = true;
            return lines;
        }

        /// <summary>
        /// Write a byte to the screen, and set new cursor position.
        /// Changes the output-flag!.
        /// </summary>
        /// <param name="writeByte">Output byte</param>
        /// <returns>True if byte has been written</returns>
        public bool WriteByte(byte writeByte)
        {
            return this.WriteByte(writeByte, true);
        }

        /// <summary>
        /// Write a byte to the screen, and set new cursor position. Changes the output-flag!
        /// </summary>
        /// <param name="writeBytes">Output bytes</param>
        /// <returns>True if byte has been written</returns>
        public bool WriteByte(byte[] writeBytes)
        {
            if (writeBytes == null || writeBytes.Length < 1) return false;
            // ReSharper disable LoopCanBeConvertedToQuery
            foreach (byte t in writeBytes)
            {
                if (!this.WriteByte(t, true)) return false;
            }
            return true;
            // ReSharper restore LoopCanBeConvertedToQuery
        }

        /// <summary>
        /// Write a byte to the screen.
        /// </summary>
        /// <remarks>
        /// Changes the output-flag!
        /// </remarks>
        /// <param name="writeByte">Output byte</param>
        /// <param name="moveCursor">Move the cursor or not</param>
        /// <returns>True if byte has been written</returns>
        public bool WriteByte(byte writeByte, bool moveCursor)
        {
            if (this._vs == null) return false;
            switch (writeByte)
            {
                case 10:
                    // NL
                    this.CursorY0++;
                    break;
                case 13:
                    // CR
                    this.CursorX0 = 0;
                    break;
                default:
                    int y = this.CursorY0;
                    if (this._visibleAreaY0Top > 0)
                        y -= this._visibleAreaY0Top;
                    if (y >= 0)
                    {
                        try
                        {
                            this._vs[this.CursorX0, y] = writeByte;
                        }
                        // ReSharper disable EmptyGeneralCatchClause
                        catch
                        {
                            // boundary problems should never occur, however
                        }
                        // ReSharper restore EmptyGeneralCatchClause
                    }
                    if (moveCursor) this.MoveCursor(1);
                    break;
            }
            this._changedScreen = true;
            return true;
        }

        /// <summary>
        /// Write a string to the screen, and set new cursor position. Changes the output-flag!
        /// </summary>
        /// <param name="s">Output string</param>
        /// <returns>True if byte has been written</returns>
        public bool WriteLine(String s)
        {
            return s != null && this.Write(s + "\n\r");
        }

        /// <summary>
        /// Write a string to the screen, and set new cursor position.
        /// </summary>
        /// <remarks>
        /// Changes the output-flag!
        /// </remarks>
        /// <param name="s">Output string</param>
        /// <returns>True if string has been written</returns>
        public bool Write(string s)
        {
            return s != null && this.WriteByte(Encoding.ASCII.GetBytes(s));
        }

        /// <summary>
        /// Write a char to the screen, and set new cursor position. Changes the output-flag!
        /// </summary>
        /// <param name="c">Output char</param>
        /// <returns>True if char has been written</returns>
        public bool Write(char c)
        {
            return this.Write(new string(c, 1));
        }

        /// <summary>
        /// Move cursor +/- positions forward. Scrolls up if necessary.
        /// </summary>
        /// <param name="positions">Positions to move (+ forward / - backwards)</param>
        public void MoveCursor(int positions)
        {
            if (positions == 0)
                return;
            int dy = positions / this.Width;
            int dx = positions - (dy * this.Width); // remaining x

            // change dx / dy if necessary
            if (dx >= 0)
            {
                // move forward
                if ((this.CursorX0 + dx) >= this.Width)
                {
                    dy++;
                    dx = dx - this.Width;
                }
            }
            else
            {
                // move backward (dx is NEGATIVE)
                if (this.CursorX0 + dx < 0)
                {
                    dy--; // one line up
                    dx = dx - this.Width;
                }
            }

            // new values:
            // do we have to scroll, line wraping for x is guaranteed
            int ny = this.CursorY0 + dy;
            int nx = this.CursorX0 + dx;
            if (ny > this._visibleAreaY0Bottom)
            {
                int sUp = ny - this._visibleAreaY0Bottom;
                this.ScrollUp(sUp);
                this._visibleAreaY0Bottom += sUp;
                this._visibleAreaY0Top = this._visibleAreaY0Bottom - this.Height - 1;
            }
            this.CursorY0 = ny;
            this.CursorX0 = nx; // since we use the PROPERTY exceeding values are cut
        }

        /// <summary>
        /// Move the cursor n rows down (+) or up(-). 
        /// </summary>
        /// <param name="lines">Number of rows up(-) or down(+)</param>
        public void MoveCursorVertical(int lines)
        {
            this.MoveCursor(lines * this.Width);
        }

        /// <summary>
        /// Move cursor to a position considering scrolling up / lines breaks.
        /// Changes the scrolledUp attribute!
        /// </summary>
        /// <param name="xPos">X Position considering offset</param>
        /// <param name="yPos">Y Position considering offset</param>
        /// <returns>true if cursor could be moved</returns>
        /// <remarks>
        /// Just to set a cursor position the attributes <see cref="CursorX"/> / <see cref="CursorY"/>
        /// could be used. This here features scrolling.
        /// </remarks>
        public bool MoveCursorTo(int xPos, int yPos)
        {
            int x0 = xPos - this._offsetx;
            int y0 = yPos - this._offsety;

            // check
            if (x0 < 0 || y0 < 0)
                return false;

            // determine extra lines because of 
            // X-Pos too high
            int dy = x0 / this.Width;
            if (dy > 0)
            {
                y0 += dy;
                x0 = x0 - (dy * this.Width);
            }

            // do we have to scroll?
            if (y0 > this._visibleAreaY0Bottom)
            {
                int sUp = y0 - this._visibleAreaY0Bottom;
                this.ScrollUp(sUp);
                this._visibleAreaY0Bottom = y0 + sUp;
                this._visibleAreaY0Top = this._visibleAreaY0Bottom - this.Height - 1;
            }

            // set values
            this.CursorX0 = x0;
            this.CursorY0 = y0;
            return true;
        }

        /// <summary>
        /// Get a line as string.
        /// </summary>
        /// <param name="yPosition"></param>
        /// <returns></returns>
        public string GetLine(int yPosition)
        {
            int y0 = yPosition - this._offsety;
            if (this._vs == null || y0 >= this.Height || this.Width < 1) return null;
            var la = new byte[this.Width];
            for (int x = 0; x < this.Width; x++)
            {
                la[x] = this._vs[x, y0];
            }
            return Encoding.ASCII.GetString(la, 0, la.Length);
        }

        /// <summary>
        /// Class info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.GetType().FullName + " " + this.Width + " | " + this.Height + " | changed " + this._changedScreen;
        }

        /// <summary>
        /// Return the values as string
        /// </summary>
        /// <returns>Screen buffer as string including NLs (newlines)</returns>
        public string Hardcopy()
        {
            return this.Hardcopy(false);
        }

        /// <summary>
        /// Return the values as string
        /// </summary>
        /// <param name="lowercase">true return as lower case</param>
        /// <returns>Screen buffer as string including NLs (newlines)</returns>
        public string Hardcopy(bool lowercase)
        {
            if (this._vs == null) return null;
            if (this._changedScreen || this._screenString == null)
            {
                int cap = this.Width * this.Height;
                var sb = new StringBuilder(cap);
                for (int y = 0; y < this.Height; y++)
                {
                    if (y > 0)
                        sb.Append('\n');
                    sb.Append(this.GetLine(y + this._offsety));
                } // for
                this._screenString = sb.ToString();
                this._changedScreen = false; // reset the flag
                if (!lowercase) return this._screenString;
                this._screenStringLower = this._screenString.ToLower();
                return this._screenStringLower;
            }
            // return from cache
            if (lowercase) return this._screenStringLower ?? (this._screenStringLower = this._screenString.ToLower());
            return this._screenString; // from cache
        }

        /// <summary>
        /// Find a string on the screen.
        /// </summary>
        /// <param name="findString">String to find</param>
        /// <param name="caseSensitive">true for case sensitive search</param>
        /// <returns>string found</returns>
        public string FindOnScreen(string findString, bool caseSensitive)
        {
            if (this._vs == null || findString == null || findString.Length < 1)
                return null;
            try
            {
                string screen = (caseSensitive) ? this.Hardcopy() : this.Hardcopy(true);
                int index = (caseSensitive) ? screen.IndexOf(findString) : screen.IndexOf(findString.ToLower());
                if (index < 0) return null;
                return caseSensitive ? findString : this.Hardcopy().Substring(index, findString.Length);
            }
            catch
            {
                // Null pointer etc.
                return null;
            }
        }

        // FindOnScreen

        /// <summary>
        /// Find a regular expression on the screen.
        /// </summary>
        /// <param name="regExp">Regular expression to find</param>
        /// <returns>string found</returns>
        public string FindRegExOnScreen(string regExp)
        {
            return this.FindRegExOnScreen(regExp, false);
        }

        /// <summary>
        /// Find a regular expression on the screen.
        /// </summary>
        /// <param name="regExp">Regular expression to find</param>
        /// <param name="caseSensitive">true for case sensitive search</param>
        /// <returns>string found</returns>
        public string FindRegExOnScreen(string regExp, bool caseSensitive)
        {
            if (this._vs == null || regExp == null || regExp.Length < 1)
                return null;
            Regex r = caseSensitive ? new Regex(regExp) : new Regex(regExp, RegexOptions.IgnoreCase);
            Match m = r.Match(this.Hardcopy()); // Remark: hardcopy uses a cache !
            return m.Success ? m.Value : null;
        }

        /// <summary>
        /// Find a regular expression on the screen
        /// </summary>
        /// <param name="regExp">Regular expression to find</param>
        /// <returns>Mathc object or null</returns>
        public Match FindRegExOnScreen(Regex regExp)
        {
            if (this._vs == null || regExp == null) return null;
            Match m = regExp.Match(this.Hardcopy()); // Remark: hardcopy uses a cache !
            return m.Success ? m : null;
        }
    } // class
    #endregion
    /// <summary>
    /// 
    /// </summary>
    static class PeriodicTaskFactory
    {
        /// <summary>
        /// Starts the periodic task.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="intervalInMilliseconds">The interval in milliseconds.</param>
        /// <param name="delayInMilliseconds">The delay in milliseconds, i.e. how long it waits to kick off the timer.</param>
        /// <param name="duration">The duration.
        /// <example>If the duration is set to 10 seconds, the maximum time this task is allowed to run is 10 seconds.</example></param>
        /// <param name="maxIterations">The max iterations.</param>
        /// <param name="synchronous">if set to <c>true</c> executes each period in a blocking fashion and each periodic execution of the task
        /// is included in the total duration of the Task.</param>
        /// <param name="cancelToken">The cancel token.</param>
        /// <param name="periodicTaskCreationOptions"><see cref="TaskCreationOptions"/> used to create the task for executing the <see cref="Action"/>.</param>
        /// <returns>A <see cref="Task"/></returns>
        /// <remarks>
        /// Exceptions that occur in the <paramref name="action"/> need to be handled in the action itself. These exceptions will not be 
        /// bubbled up to the periodic task.
        /// </remarks>
        public static Task Start(Action action,
                                     int intervalInMilliseconds = Timeout.Infinite,
                                     int delayInMilliseconds = 0,
                                     int duration = Timeout.Infinite,
                                     int maxIterations = -1,
                                     bool synchronous = false,
                                     CancellationToken cancelToken = new CancellationToken(),
                                     TaskCreationOptions periodicTaskCreationOptions = TaskCreationOptions.None)
        {
            System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
            Action wrapperAction = () =>
            {
                CheckIfCancelled(cancelToken);
                action();
            };

            Action mainAction = () =>
            {
                MainPeriodicTaskAction(intervalInMilliseconds, delayInMilliseconds, duration, maxIterations, cancelToken, stopWatch, synchronous, wrapperAction, periodicTaskCreationOptions);
            };

            return Task.Factory.StartNew(mainAction, cancelToken, TaskCreationOptions.LongRunning, TaskScheduler.Current);
        }

        /// <summary>
        /// Mains the periodic task action.
        /// </summary>
        /// <param name="intervalInMilliseconds">The interval in milliseconds.</param>
        /// <param name="delayInMilliseconds">The delay in milliseconds.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="maxIterations">The max iterations.</param>
        /// <param name="cancelToken">The cancel token.</param>
        /// <param name="stopWatch">The stop watch.</param>
        /// <param name="synchronous">if set to <c>true</c> executes each period in a blocking fashion and each periodic execution of the task
        /// is included in the total duration of the Task.</param>
        /// <param name="wrapperAction">The wrapper action.</param>
        /// <param name="periodicTaskCreationOptions"><see cref="TaskCreationOptions"/> used to create a sub task for executing the <see cref="Action"/>.</param>
        public static void MainPeriodicTaskAction(int intervalInMilliseconds,
                                                       int delayInMilliseconds,
                                                       int duration,
                                                       int maxIterations,
                                                       CancellationToken cancelToken,
                                                       System.Diagnostics.Stopwatch stopWatch,
                                                       bool synchronous,
                                                       Action wrapperAction,
                                                       TaskCreationOptions periodicTaskCreationOptions)
        {
            TaskCreationOptions subTaskCreationOptions = TaskCreationOptions.AttachedToParent | periodicTaskCreationOptions;
            CheckIfCancelled(cancelToken);
            if (delayInMilliseconds > 0)
            {
                Thread.Sleep(delayInMilliseconds);
            }
            if (maxIterations == 0) { return; }
            int iteration = 0;
            ////////////////////////////////////////////////////////////////////////////
            // using a ManualResetEventSlim as it is more efficient in small intervals.
            // In the case where longer intervals are used, it will automatically use 
            // a standard WaitHandle....
            // see http://msdn.microsoft.com/en-us/library/vstudio/5hbefs30(v=vs.100).aspx
            using (ManualResetEventSlim periodResetEvent = new ManualResetEventSlim(false))
            {
                ////////////////////////////////////////////////////////////
                // Main periodic logic. Basically loop through this block
                // executing the action
                while (true)
                {
                    CheckIfCancelled(cancelToken);
                    Task subTask = Task.Factory.StartNew(wrapperAction, cancelToken, subTaskCreationOptions, TaskScheduler.Current);
                    if (synchronous)
                    {
                        stopWatch.Start();
                        try
                        {
                            subTask.Wait(cancelToken);
                        }
                        catch { /* do not let an errant subtask to kill the periodic task...*/ }
                        stopWatch.Stop();
                    }
                    // use the same Timeout setting as the System.Threading.Timer, infinite timeout will execute only one iteration.
                    if (intervalInMilliseconds == Timeout.Infinite) { break; }
                    iteration++;
                    if (maxIterations > 0 && iteration >= maxIterations) { break; }
                    try
                    {
                        stopWatch.Start();
                        periodResetEvent.Wait(intervalInMilliseconds, cancelToken);
                        stopWatch.Stop();
                    }
                    finally
                    {
                        periodResetEvent.Reset();
                    }
                    CheckIfCancelled(cancelToken);
                    if (duration > 0 && stopWatch.ElapsedMilliseconds >= duration) { break; }
                }
            }
        }
        /// <summary>
        /// Checks if cancelled.
        /// </summary>
        /// <param name="cancelToken">The cancel token.</param>
        public static void CheckIfCancelled(CancellationToken cancellationToken)
        {
            if (cancellationToken == null)
                throw new ArgumentNullException("cancellationToken");
            cancellationToken.ThrowIfCancellationRequested();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    class Telnet
    {
        public static object _lock = new object();
        /// <summary>
        /// 
        /// </summary>
        static Terminal tn;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private static string TelnetCommand(string command)
        {
            string result = string.Empty;
            try
            {
                tn = new Terminal(AppConfiguration.SERVER);
                tn.Connect();
                do
                {
                    string f = tn.WaitForString("Login");
                    if (f == null)
                        throw new TerminalException("No login possible");
                    Console.WriteLine(tn.VirtualScreen.Hardcopy().TrimEnd());
                    tn.SendResponse(AppConfiguration.USERNAME, true); // send username
                    f = tn.WaitForString("Password");
                    if (f == null)
                        throw new TerminalException("No password prompt found");
                    Console.WriteLine(tn.VirtualScreen.Hardcopy().TrimEnd());
                    tn.SendResponse(AppConfiguration.PASSWORD, true); // send password 
                    f = tn.WaitForString(">");
                    if (f == null)
                        throw new TerminalException("No > prompt found");
                    tn.SendResponse(command, true); // execute command
                    if (tn.WaitForChangedScreen())
                    {
                        result=tn.VirtualScreen.Hardcopy().TrimEnd();
                        Console.WriteLine(result);
                    }
                }
                while (false);
                tn.Close();

            }
            catch
            {

                throw;
            }
            return result;
        }
        private static string TelnetCommand(string[] command)
        {
            string result = string.Empty;
            try
            {
                tn = new Terminal(AppConfiguration.SERVER);
                tn.Connect();
                do
                {
                    string f = tn.WaitForString("Login");
                    if (f == null)
                        throw new TerminalException("No login possible");
                    Console.WriteLine(tn.VirtualScreen.Hardcopy().TrimEnd());
                    tn.SendResponse(AppConfiguration.USERNAME, true); // send username
                    f = tn.WaitForString("Password");
                    if (f == null)
                        throw new TerminalException("No password prompt found");
                    Console.WriteLine(tn.VirtualScreen.Hardcopy().TrimEnd());
                    tn.SendResponse(AppConfiguration.PASSWORD, true); // send password 
                    f = tn.WaitForString(">");
                    if (f == null)
                        throw new TerminalException("No > prompt found");
                    foreach (var c in command)
                    {
                      tn.SendResponse(c, true); // execute command
                    }
                    if (tn.WaitForChangedScreen())
                    {
                        result = tn.VirtualScreen.Hardcopy().TrimEnd();
                        Console.WriteLine(result);
                    }
                }
                while (false);
                tn.Close();

            }
            catch
            {

                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private static string SSHCommand(string[] command)
        {

            string result = string.Empty;
            try
            {
                //don't remove these three variables
                var input = new MemoryStream();
                var output = new MemoryStream();
                var exOtput = new MemoryStream();
                using (var ssh = new SshClient(CreateConnectionInfo()))
                {
                    ssh.Connect();
                    using (ShellStream shell = ssh.CreateShellStream("dumb", 80, 24, 800, 600, 1024))
                    {
                        foreach (string c in command)
                        {
                            result = SendCommand(c, shell);
                            Console.WriteLine(result);
                        }
                        shell.Close();
                    }
                    ssh.Disconnect();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        private static string SSHCommand(string command)
        {
            string result = string.Empty;
            try
            {
                //don't remove these three variables
                var input = new MemoryStream();
                var output = new MemoryStream();
                var exOtput = new MemoryStream();
                using (var ssh = new SshClient(CreateConnectionInfo()))
                {
                    ssh.Connect();
                    using (ShellStream shell = ssh.CreateShellStream("dumb", 80, 24, 800, 600, 1024))
                    {
                        result = SendCommand(command, shell);
                        Console.WriteLine(result);
                        shell.Close();
                    }
                    ssh.Disconnect();
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        public static string Execute(string command)
        {
            string result = string.Empty;
            try
            {
                lock (_lock)
                {
                    switch (AppConfiguration.PORT)
                    {
                        case 22:result= SSHCommand(command); break;
                        case 23:result= TelnetCommand(command); break;
                        default: break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static string Execute(string[] command)
        {
            string result = string.Empty;
            try
            {
                lock (_lock)
                {
                    switch (AppConfiguration.PORT)
                    {
                        case 22: result = SSHCommand(command); break;
                        case 23: result = TelnetCommand(command); break;
                        default: break;
                    }
                }
            }
            catch
            {
                throw;
            }
            return result;
        }

       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="sh"></param>
        /// <returns></returns>
        public static string SendCommand(string cmd, ShellStream sh)
        {
            StreamReader reader = null;
            try
            {
                reader = new StreamReader(sh);
                StreamWriter writer = new StreamWriter(sh);
                writer.AutoFlush = true;
                writer.WriteLine(cmd);
                while (sh.Length == 0)
                {
                    Thread.Sleep(30000);
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            return reader.ReadToEnd();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static ConnectionInfo CreateConnectionInfo()
        {
            KeyboardInteractiveAuthenticationMethod keybAuth = new KeyboardInteractiveAuthenticationMethod(AppConfiguration.USERNAME);
            keybAuth.AuthenticationPrompt += new EventHandler<AuthenticationPromptEventArgs>(HandleKeyEvent);
            return new ConnectionInfo(AppConfiguration.SERVER, AppConfiguration.PORT, AppConfiguration.USERNAME, keybAuth);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void HandleKeyEvent(object sender, AuthenticationPromptEventArgs e)
        {
            foreach (AuthenticationPrompt prompt in e.Prompts)
            {
                if (prompt.Request.IndexOf("Password:", StringComparison.InvariantCultureIgnoreCase) != -1)
                {
                    prompt.Response = AppConfiguration.PASSWORD;
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    class DoProcess : IDisposable
    {
        
        /// <summary>
        /// 
        /// </summary>
        public readonly string database = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        private  ProcessItem _process;
        private readonly System.Data.IDbConnection _connection;
        /// <summary>
        /// 
        /// </summary>
        private readonly WD.DataAccess.Context.DbContext _dbContext = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        public DoProcess(ProcessItem process,ref  System.Data.IDbConnection dbConnection,ref  WD.DataAccess.Context.DbContext dbContext)
        {
            _dbContext = dbContext;
            _process = process;
            _connection = dbConnection;
            database = _dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("Database")), Encoding.UTF8), _connection).ToString();
        }
        bool isDelay = false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        public  void DoTalking(object duration,string currentDate)
        {
               {
                   try
                   {
                       DateTime endTime = DateTime.Now.AddMilliseconds(Convert.ToDouble(duration));
                       DateTime currentTime = DateTime.Now;
                       CycleFirst(currentDate);
                       if (isDelay)
                       {
                           currentDate = _dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8), _connection).ToString();
                           bool flag = false;
                           int count = 0;
                           do
                           {
                               count++;
                               if (CheckProcess())
                               {
                                   flag = true;
                                   break;
                               }
                               Thread.Sleep(AppConfiguration.RUNNINGDELAY);
                           } while (count < 5);
                           Console.WriteLine("It took {0} round(s) to check {1} is running at {2}.", count, _process.ProcessCode, DateTime.Now);
                           //send email if process is not started or go for cycle 2 if process started
                           if (flag == false)
                           {
                               SendEmailOrSms();
                           }
                           else
                           {
                               CycleSecond();
                             
                           }
                           Console.WriteLine("Delay Starts at {0}", DateTime.Now);
                           Thread.Sleep(AppConfiguration.DELAY);
                           Console.WriteLine("Process {0} {1} Delay Starts at {2}.", _process.ProcessCode, _process.Index, DateTime.Now);
                           Console.WriteLine("Process {0} {1} Delay Ends at {2}.", _process.ProcessCode, _process.Index, DateTime.Now);
                       }
                   }
                   catch (Exception exc)
                   {
                       Console.WriteLine(exc);
                       WD.DataAccess.Logger.ILogger.Error(exc);
                   }
               }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CycleFirst(string currentDate)
        {
            Console.WriteLine("Cycle 1 for {0} Starts at {1}.", _process.ProcessCode, DateTime.Now);
            int count = 0;
            pList = new List<ProcessItem>();
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                count++;
                Store(currentDate);
                Console.WriteLine("Process {0} Cycle 1 Interval {1} {2}",_process.ProcessCode, count, DateTime.Now);
            }, intervalInMilliseconds: AppConfiguration.CYCLE1INTERVAL, maxIterations: 3);
            perdiodicTask.ContinueWith(_ =>
            {
                Console.WriteLine("No Of Delays {0} for Cycle 1.", pList.Count());
                if (pList.Count() > 0)
                {
                    isDelay = true;
                    Console.WriteLine("Delays present for {0} at{1}.", _process.ProcessCode, DateTime.Now);
                    if (_process.Dependency == 0)
                    {
                        Console.WriteLine("Operations Start for {0} at{1}.", _process.ProcessCode, DateTime.Now);
                        Stop();//stop process
                        Wait();//wait for 2 mins
                        Console.WriteLine("Check Running for Kill {0} at{1}.", _process.ProcessCode, DateTime.Now);
                        if (CheckRunning())//if process is runnning
                        {

                            Kill();//kill process if still alive
                            Thread.Sleep(AppConfiguration.KILLDELAY);//20 second delay
                            Start();//start process
                        }
                        else
                        {
                            Start();//start process
                        }
                        Console.WriteLine("Starting Process {0} at{1}.", _process.ProcessCode, DateTime.Now);
                        _killflag = 1;
                    }
                    else
                    {
                        SendEmailOrSms();//send email if process is having dependency
                        _killflag = 1;
                    }
                }
            }).Wait();
            pList=null;
            Console.WriteLine("Cycle 1 for {0} Ends at {1}.", _process.ProcessCode, DateTime.Now);
        }
        private void SendEmailOrSms()
        {
            try
            {
                switch (AppConfiguration.EMAILORSMS)
                {
                    case "email":
                        SendEmail();
                        break;
                    case "sms":
                        SendSms('S');
                        break;
                    default:
                        SendEmail();
                        SendSms('S');
                        break;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        private void CycleSecond()
        {
            string currentDate = _dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8),_connection).ToString();
            Thread.Sleep(40000);
            Console.WriteLine("Cycle 2 for {0} Starts at {1}.", _process.ProcessCode, DateTime.Now);
            int count = 0;
            pList = new List<ProcessItem>();
            Task perdiodicTask = PeriodicTaskFactory.Start(() =>
            {
                count++;
                Console.WriteLine("Process {0} Cycle 2 Interval {1} {2}", _process.ProcessCode, count, DateTime.Now);
                Store(currentDate);
            }, intervalInMilliseconds: AppConfiguration.CYCLE2INTERVAL, maxIterations: 3);
            perdiodicTask.ContinueWith(_ =>
            {
                Console.WriteLine("No Of Delays {0} for Cycle 2.", pList.Count());
                if (pList.Count() > 0)
                {
                    SendEmailOrSms();
                    _killflag = 2;
                }
            }).Wait();
            pList = null;
            Console.WriteLine("Cycle 2 for {0} Ends at {1}.", _process.ProcessCode, DateTime.Now);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckSqlText()
        {
            bool result = false;
            try
            {
                ProcessItem p = _dbContext.ICommands.GetEntity<ProcessItem>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessCheckRunning")), Encoding.UTF8), _connection, new WD.DataAccess.Parameters.DBParameter[] { new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode } });
                if (p != null)
                {
                    _process.Sid = p.Sid;
                    _process.SpId = p.SpId;
                    result =  p.SqlText.ToUpper().Equals(_process.SqlText.ToUpper()) ? true : false;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckProcess()
        {
            bool result = false;
            try
            {
                ProcessItem p = _dbContext.ICommands.GetEntity<ProcessItem>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessCheck")), Encoding.UTF8), _connection, new WD.DataAccess.Parameters.DBParameter[] { new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode } });
                if (p != null)
                {
                    _process.Sid = p.Sid;
                    _process.SpId = p.SpId;
                    result = true;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            return result;
        }
        private bool CheckRunning()
        {
            bool result = false;
            try
            {
                ProcessItem p = _dbContext.ICommands.GetEntity<ProcessItem>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessCheckRunning")), Encoding.UTF8), _connection, new WD.DataAccess.Parameters.DBParameter[] 
                { 
                    new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode } ,
                });
                if (p != null)
                {
                    Console.WriteLine("Process {0} running at {1}.", _process.ProcessCode, DateTime.Now);
                    result = true;
                }
                else
                {
                    Console.WriteLine("Process {0} not running at {1}.", _process.ProcessCode, DateTime.Now);
                    result = false;
                }

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        private List<ProcessItem> pList = null;
        /// <summary>
        /// 
        /// </summary>
        private void Store(string createDate)
        {

            List<WD.DataAccess.Parameters.DBParameter> aParam = new List<DataAccess.Parameters.DBParameter>();
            aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode });
            aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Sid", ParameterValue = _process.Sid });
            aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Dependency", ParameterValue = _process.Dependency });
            aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "CreateDate",  ParameterValue = createDate });
            ProcessItem p = _dbContext.ICommands.GetEntity<ProcessItem>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessGet")), Encoding.UTF8), _connection, aParam.ToArray());
            if (p != null)
            {
               Console.WriteLine("Process {0} delay {1} spid {2}", p.ProcessCode, p.Delay, _process.SpId);
               pList.Add(p);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int _killflag = 0;
        /// <summary>
        /// 
        /// </summary>
        private void Start()
        {
            try
            {
                Console.WriteLine("Starting {0}.", _process.ProcessCode);
                StartStop('C');
                Telnet.Execute(new string[] { System.Configuration.ConfigurationManager.AppSettings.Get("UnixScriptPath") + _process.UnixScript, @"nohup " + System.Configuration.ConfigurationManager.AppSettings.Get("UnixScriptPath") + _process.UnixScript + " &" });
                UpdateDatabase(1, "Start");
               _killflag = 1;//so that cycles 2 gets started
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void Stop()
        {

            try
            {
                Console.WriteLine("Stopping {0} at {1}.", _process.ProcessCode, DateTime.Now);
                StartStop('T');
                UpdateDatabase(1, "Stop");
                Thread.Sleep(5000);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private void Wait()
        {
            Console.WriteLine("Delay of 2 mins for {0} at {1}.", _process.ProcessCode, DateTime.Now);
            UpdateDatabase(1, "Wait");
            Thread.Sleep(AppConfiguration.WAIT);
        }
        /// <summary>
        /// 
        /// </summary>
        private string Kill()
        {
            string result = string.Empty;
            try
            {
                Console.WriteLine("Killing {0} at {1}.", _process.ProcessCode, DateTime.Now);
                UpdateDatabase(1, "Kill");
                result = Telnet.Execute(@"kill -9 " + _process.SpId);
               _killflag = 1;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        private void SendEmail()
        {
            try
            {
                Console.WriteLine("Sending Email for {0} at {1}.", _process.ProcessCode, DateTime.Now);
                {
                    using (System.Net.Mail.MailMessage mailMessage = new System.Net.Mail.MailMessage())
                    {
                        mailMessage.From = new System.Net.Mail.MailAddress(ConfigurationManager.AppSettings.Get("SmtpFrom"));
                        foreach (var address in ConfigurationManager.AppSettings.Get("SmtpTo").Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                        {
                            mailMessage.To.Add(address);
                        }
                        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
                        {
                            Host = ConfigurationManager.AppSettings.Get("SmtpHost"),
                            Port = WD.DataAccess.Helpers.HelperUtility.ConvertTo<int>(ConfigurationManager.AppSettings.Get("SmtpPort"), 25),
                            EnableSsl = false,
                            DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                            Timeout = 10000,
                        };
                        mailMessage.IsBodyHtml = true;
                        mailMessage.Subject = ConfigurationManager.AppSettings.Get("SmtpSubject");
                        mailMessage.Body = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("SmtpBody")), Encoding.UTF8).Replace("#Database", database).Replace("#ProcessCode", _process.ProcessCode).Replace("#SPID", _process.SpId.ToString()).Replace("#Delay", pList == null ? _process.Delay.ToString() : pList.Max(x => x.Delay).ToString());
                        client.Send(mailMessage);
                        Console.WriteLine("Email Send for {0} at {1}.", _process.ProcessCode, DateTime.Now);
                        
                    }
                }
                UpdateDatabase(2, "Email");
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
        }
        private void SendSms(char type)
        {

            try
            {
               
                Console.WriteLine("Sending Sms for {0} at {1}.", _process.ProcessCode, DateTime.Now);
                string message = _process.ProcessCode + "," + database + ",Delay=" + (pList == null ? _process.Delay.ToString() : pList.Max(x => x.Delay).ToString());
                foreach (var to in ConfigurationManager.AppSettings.Get("SmsTo").Split(','))
                {
                    List<WD.DataAccess.Parameters.DBParameter> aParam = new List<DataAccess.Parameters.DBParameter>();
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "m_type", ParameterValue = type });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "m_from", ParameterValue = database });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "m_contact", ParameterValue = to });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "m_string", ParameterValue = message });
                    _dbContext.ICommands.ExecuteNonQuery(ConfigurationManager.AppSettings.Get("SmsProcedure"), System.Data.CommandType.StoredProcedure, _connection, aParam.ToArray());
                }
                message = string.Empty;
                Console.WriteLine("Sms Send for {0} at {1}.", _process.ProcessCode, DateTime.Now);
            }

            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        private void StartStop(char code)
        {
            try
            {
                List<WD.DataAccess.Parameters.DBParameter> aParam = new List<DataAccess.Parameters.DBParameter>();
                string theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessUpdate")), Encoding.UTF8);
                aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "CODE", ParameterValue = code });
                aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCODE", ParameterValue = _process.ProcessCode });
                _dbContext.ICommands.ExecuteNonQuery(theSQL, _connection, aParam.ToArray());
                aParam = null;
            }
            catch
            {

                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cycleNo"></param>
        /// <param name="action"></param>
        private void UpdateDatabase(int cycleNo, string action)
        {
            try
            {
                string theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("TransactionGet")), Encoding.UTF8);
                List<WD.DataAccess.Parameters.DBParameter> aParam = new List<DataAccess.Parameters.DBParameter>();
                aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode });
                if (_dbContext.ICommands.ExecuteScalar(theSQL, _connection, aParam.ToArray()).ToString() == "0")
                {
                    theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("TransactionInsert")), Encoding.UTF8);
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "SpId", ParameterValue = _process.SpId });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Dependency", ParameterValue = _process.Dependency });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Status", ParameterValue = action });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "CycleNo", ParameterValue = cycleNo });
                    _dbContext.ICommands.ExecuteNonQuery(theSQL, _connection, aParam.ToArray());
                }
                else
                {
                    theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("HistoryInsert")), Encoding.UTF8);
                    _dbContext.ICommands.ExecuteNonQuery(theSQL, _connection, new[] { new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode } });
                    theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("TransactionDelete")), Encoding.UTF8);
                    _dbContext.ICommands.ExecuteNonQuery(theSQL, _connection, new[] { new WD.DataAccess.Parameters.DBParameter() { ParameterName = "PCode", ParameterValue = _process.ProcessCode } });

                    theSQL = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("TransactionInsert")), Encoding.UTF8);
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "SpId", ParameterValue = _process.SpId });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Dependency", ParameterValue = _process.Dependency });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "Status", Type = System.Data.DbType.AnsiString, ParameterValue = action });
                    aParam.Add(new WD.DataAccess.Parameters.DBParameter() { ParameterName = "CycleNo", Type = System.Data.DbType.Int32, ParameterValue = cycleNo });
                    _dbContext.ICommands.ExecuteNonQuery(theSQL, _connection, aParam.ToArray());
                }
                theSQL = string.Empty;
            }
            catch { throw; }
        }
        public void Dispose()
        {
            _process = null;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    class Program
    {
        #region Trap application termination
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);
        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig) {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");
            //do your cleanup here
            Thread.Sleep(60000); //simulate some cleanup delay
            Dispose();
            Console.WriteLine("Cleanup complete");
            //allow main to run off
            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);
            return true;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        private static WD.DataAccess.Context.DbContext dbContext = null;
        private static System.Data.IDbConnection dbConnection = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                _handler += new EventHandler(Handler);
                SetConsoleCtrlHandler(_handler, true);
                Calculate();
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);
            }
            Console.Read();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        private async static void SetProcess(double duration)
        {
            await Task.Factory.StartNew(() =>
            {
                int count = 0;
                DateTime currentTime = DateTime.Now;
                DateTime endTime = DateTime.Now.AddMilliseconds(duration);
                while (currentTime <= endTime)
                {
                    try
                    {
                        count = count + dbContext.ICommands.ExecuteNonQuery(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessInsert")), Encoding.UTF8), dbConnection);
                        Console.WriteLine("{0} Records Added On {1}", count, DateTime.Now);
                        Thread.Sleep(AppConfiguration.INSERT);
                        currentTime = DateTime.Now;
                    }
                    catch (Exception exc)
                    {
                        Console.WriteLine(exc.Message);
                    }
                }
                count = 0;
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="duration"></param>
        private static void GetProcess(double duration,string currentDate)
        {
            try
            {
               
                if (dbConnection != null)
                {
                    Console.WriteLine("Cycle Calculation Starting for Processe(s)");
                    int count = 1;
                    List<ProcessItem> pList = dbContext.ICommands.GetList<ProcessItem>(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("ProcessGetList")), Encoding.UTF8), dbConnection, new[] { new WD.DataAccess.Parameters.DBParameter() { ParameterName = "CreateDate", ParameterValue = currentDate } });
                    List<Task> t = new List<Task>();
                    Parallel.ForEach(pList, (p) =>
                    {
                        p.Index = count;
                        Console.WriteLine(p.ProcessCode + " in loop {0}", count);
                        using (DoProcess doProc = new DoProcess(p, ref dbConnection, ref dbContext))
                        {
                            doProc.DoTalking(duration, currentDate);
                        }
                        Interlocked.Increment(ref count);
                    });
                    pList = null;
                    Console.WriteLine("Cycle Calculation Ended for Processe(s)");
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
                WD.DataAccess.Logger.ILogger.Error(exc);

            }

        }
        /// <summary>
        /// 
        /// </summary>
        private static async void Calculate()
        {
            await Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Start {0}", DateTime.Now);
                try
                {
                    dbContext = new DataAccess.Context.DbContext();
                    dbConnection = dbContext.ICommands.CreateConnection();
                    string currentDate = dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8), dbConnection).ToString();
                    DateTime startTime = DateTime.ParseExact(AppConfiguration.CYCLESTART, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture); // new TimeSpan(AppConfiguration.CYCLESTART, 0, 0);
                    DateTime endTime = DateTime.ParseExact(AppConfiguration.CYCLEEND, "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture); // new TimeSpan(AppConfiguration.CYCLESTART, 0, 0);
                    if (startTime >= endTime)
                    {
                        endTime = endTime.AddDays(1);

                    }
                    DateTime currentTime = DateTime.Now;
                    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
                    double duration = 0;
                    if ((currentDay == DayOfWeek.Saturday))
                    {
                        currentTime = DateTime.Now;
                        Console.WriteLine("Todays process tasks started at {0}", DateTime.Now);
                        duration = ((23) * 60 * 60 * 1000);
                       
                        SetProcess(duration);
                        Console.WriteLine("Todays process will end at {0}", endTime);
                        while ((currentDay == DayOfWeek.Saturday))
                        {
                            GetProcess(duration, currentDate);
                            currentDay = DateTime.Now.DayOfWeek;
                            currentDate = dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8), dbConnection).ToString();
                        }
                        Console.WriteLine("Todays process tasks completed at {0}", DateTime.Now);
                    }
                    else if ((currentDay == DayOfWeek.Sunday))
                    {
                        currentTime = DateTime.Now;
                        Console.WriteLine("Todays process tasks started at {0}", DateTime.Now);
                        duration = (23) * 60 * 60 * 1000;
                        SetProcess(duration);
                        Console.WriteLine("Todays process will end at {0}", endTime);
                        while ((currentDay == DayOfWeek.Sunday))
                        {
                            GetProcess(duration, currentDate);
                            currentDay = DateTime.Now.DayOfWeek;
                            currentDate = dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8), dbConnection).ToString();
                        }
                        Console.WriteLine("Todays process tasks completed at {0}", DateTime.Now);
                    }
                    else if (currentTime >= startTime && currentTime <= endTime)
                    {
                        currentTime = DateTime.Now;
                        Console.WriteLine("Todays process tasks started at {0}", DateTime.Now);
                        duration = (endTime - currentTime).TotalMilliseconds;
                        SetProcess(duration);
                        Console.WriteLine("Todays process will end at {0}", endTime);
                        while (currentTime >= startTime && currentTime <= endTime)
                        {
                            duration = (endTime - currentTime).TotalMilliseconds;
                            GetProcess(duration, currentDate);
                            currentTime = DateTime.Now;
                            currentDate = dbContext.ICommands.ExecuteScalar(File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, System.Configuration.ConfigurationManager.AppSettings.Get("CurrentDate")), Encoding.UTF8), dbConnection).ToString();
                        }
                        Console.WriteLine("Todays process tasks completed at {0}", DateTime.Now);
                    }
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc);
                    WD.DataAccess.Logger.ILogger.Error(exc);
                }
                finally
                {
                    Console.WriteLine("End {0}", DateTime.Now);
                    dbConnection.Close();
                    dbConnection.Dispose();
                    dbContext.Dispose();
                }
            });
            Thread.Sleep(1000);
            Calculate();
        }
        public static void Dispose()
        {
            if (dbConnection != null) {

                if (dbConnection.State == System.Data.ConnectionState.Open)
                {
                    dbConnection.Close();
                }
                dbConnection.Dispose();
            }
            dbContext.Dispose();
        }
    }
    
}
