::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::: 
:: Author: Goh Kok Keong (Mesa Consultant)
:: Edited by Leion 19 Oct 2011
:: Date Update: 07 Jun 2004 
:: Description: Register related components for XTI project
::
:: Steps :
:: 1: Change to .NET Framework working directory 
:: 2: Register .NET Components to GAC ( Global Assembly Cache ).
::
:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

cls

@echo -
@echo ---------------------------------------------------
@echo Step 1: Change to .NET Framework working directory 
@echo ---------------------------------------------------
@echo -
@echo -
@echo ------------------------------------------------------------------
@echo Step 2: Register .NET Components to GAC ( Global Assembly Cache ).
@echo ------------------------------------------------------------------
@echo -
gacutil /u WD.DataAccess

@echo -
@echo ---------------------------------------------------
@echo Step 1: Change to .NET Framework working directory 
@echo ---------------------------------------------------
@echo -
@echo -
@echo ------------------------------------------------------------------
@echo Step 2: Register .NET Components to GAC ( Global Assembly Cache ).
@echo ------------------------------------------------------------------
@echo -
