// ***********************************************************************
// Assembly         : WD.DataAccess
// Author           : shahid_k
// Created          : 01-23-2017
//
// Last Modified By : shahid_k
// Last Modified On : 07-04-2017
// ***********************************************************************
// <copyright file="QueryBuilder.cs" company="Western Digital">
//     Copyright © Western Digital 2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using WD.DataAccess.Attributes;
using WD.DataAccess.Helpers;



// namespace: WD.DataAccess.QueryProviders
//
// summary:	.


namespace WD.DataAccess.QueryProviders
{
   

    /// <summary>   A query builder. </summary>
    ///
    /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
    ///
    /// <typeparam name="T">    . </typeparam>
    

    public class QueryBuilder<T>
    {
        
        /// <summary>   Default constructor. </summary>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        

        public QueryBuilder()
        {
            _where = null;
            _groupBy = new List<string>();
            _orderBy = new List<string>();
            _projection = new List<string>();
            _sum = new List<string>();
            _count = new List<string>();
            _join = new List<KeyValuePair<Type, Tuple<string, string>>>();
        }

        /// <summary>   The tt. </summary>
        T TT;
        /// <summary>   Default constructor. </summary>
        ///<example>
        ///<code>
        ///public class Student
        ///{
        ///    public int StudentId { get; set; }
        ///    public string FirstName { get; set; }
        ///    public string LastName { get; set; }

        ///}
        ///public class StudentCourse
        ///{

        ///    public int StudentId { get; set; }
        ///    public int CourseId { get; set; }
        ///    public string Name { get; set; }
        ///}
        ///public class TestClass
        ///{
        ///    static void Main()
        ///    {
        ///        QueryBuilder<Student> qb = new QueryBuilder<Student>();
        ///        string insertQuery = qb.Insert();
        ///        string selectQuery = qb.Select();
        ///        selectQuery = qb.Where(x => x.StudentId == 3).Select();
        ///        selectQuery = qb.Join<StudentCourse>(s => s.StudentId, sc => sc.StudentId).Select();
        ///        string updateQuery = qb.Update();
        ///        string whereCluase = qb.Where();
        ///        string deleteQuery = qb.Delete();
        ///    }
        ///}
        /// </code>
        /// </example>
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        /// <param name="input">    . </param>
        

        public QueryBuilder(T input)
        {
            _where = null;
            _groupBy = new List<string>();
            _orderBy = new List<string>();
            _projection = new List<string>();
            _sum = new List<string>();
            _count = new List<string>();
            _join = new List<KeyValuePair<Type, Tuple<string, string>>>();
            TT = input;
        }

        
        /// <summary>   Gets or sets the where. </summary>
        ///
        /// <value> The where. </value>
        

        private Expression<Func<T, bool>> _where { get; set; }

        
        /// <summary>   Gets or sets the amount to group by. </summary>
        ///
        /// <value> Amount to group by. </value>
        

        private List<string> _groupBy { get; set; }

        
        /// <summary>   Gets or sets the amount to order by. </summary>
        ///
        /// <value> Amount to order by. </value>
        

        private List<string> _orderBy { get; set; }

        
        /// <summary>   Gets or sets the projection. </summary>
        ///
        /// <value> The projection. </value>
        

        private List<string> _projection { get; set; }

        
        /// <summary>   Gets or sets the number of.  </summary>
        ///
        /// <value> The sum. </value>
        

        private List<string> _sum { get; set; }

        
        /// <summary>   Gets or sets the number of.  </summary>
        ///
        /// <value> The count. </value>
        

        private List<string> _count { get; set; }

        
        /// <summary>   Gets or sets the join. </summary>
        ///
        /// <value> The join. </value>
        

        private List<KeyValuePair<Type, Tuple<string, string>>> _join { get; set; }

        
        /// <summary>   Gets or sets the limit. </summary>
        ///
        /// <value> The limit. </value>
        

        private int _limit { get; set; }

        
        /// <summary>   Gets or sets the offset. </summary>
        ///
        /// <value> The offset. </value>
        

        private int _offset { get; set; }

        
        /// <summary>   Gets the select. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Select()
        {
            return string.Format("SELECT {0}{1}{2} FROM {3}  {4} {5} {6} {7} ", Projection(), Sum(), Count(),
             Helpers.HelperUtility.GetTableName<T>(), Join(), Where(), GroupBy(), OrderBy());
        }

        
        /// <summary>   Gets the insert. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Insert()
        {
            List<String> columnList = new List<string>();
            List<Object> valueList = new List<Object>();
            foreach (PropertyInfo propertyInfo in TT.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(TT, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(TT, null);
                switch (propertyInfo.PropertyType.ToString())
                {
                    case "System.DateTime":
                        if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                        {
                            columnList.Add(HelperUtility.ColumnName(propertyInfo));
                            valueList.Add("'" + value + "'");
                        }
                        break;
                    case "System.String":
                    case "System.Byte":
                    case "System.Char":
                        columnList.Add(HelperUtility.ColumnName(propertyInfo));
                        valueList.Add("'" + value + "'");
                        break;
                    default:
                        columnList.Add(HelperUtility.ColumnName(propertyInfo));
                        valueList.Add(value);
                        break;
                }
              
            }
           return string.Format("INSERT INTO {0} ({1}) VALUES ({2})",
                            HelperUtility.GetTableName<T>(),
                            string.Join(",", columnList.ToArray()),
                            string.Join(",", valueList.ToArray())
                            );
        }

        
        /// <summary>   Updates this object. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Update()
        {
            List<Object> setValues = new List<Object>();
            List<Object> whereClause = new List<Object>();
            foreach (PropertyInfo propertyInfo in TT.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(TT, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(TT, null);
                switch (propertyInfo.PropertyType.ToString())
                {
                    case "System.DateTime":
                        if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                        {
                            if (!HelperUtility.IsPrimary(propertyInfo))
                            {
                                setValues.Add(HelperUtility.ColumnName(propertyInfo) + "='" + value + "'");

                            }
                            else
                            {
                                whereClause.Add(HelperUtility.ColumnName(propertyInfo) + "='" + value + "'");
                            }
                        }
                        break;
                    case "System.String":
                    case "System.Byte":
                    case "System.Char":
                        if (!HelperUtility.IsPrimary(propertyInfo))
                        {
                            setValues.Add(HelperUtility.ColumnName(propertyInfo) + "='" + value + "'");

                        }
                        else
                        {
                            whereClause.Add(HelperUtility.ColumnName(propertyInfo) + "='" + value + "'");
                        }
                        break;
                    default:
                        if (!HelperUtility.IsPrimary(propertyInfo))
                        {
                            setValues.Add(HelperUtility.ColumnName(propertyInfo) + "=" + value);
                        }
                        else
                        {
                            whereClause.Add(HelperUtility.ColumnName(propertyInfo) + "=" + value);
                        }
                        break;
                }

            }
            return string.Format("UPDATE {0} SET {1} WHERE {2}",
                             HelperUtility.GetTableName<T>(),
                             string.Join(",", setValues.ToArray()),
                             string.Join(" AND ", whereClause.ToArray())
                             );
        }

        
        /// <summary>   Deletes this object. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Delete()
        {
            List<Object> whereClause =  new List<Object>();
            foreach (PropertyInfo propertyInfo in TT.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty).Where(x => x.GetValue(TT, null) != null).Where(x => x.GetMethod.IsVirtual == false).Where(x => x.GetCustomAttribute<CustomAttribute>() == null || x.GetCustomAttribute<CustomAttribute>().NotMapped == false))
            {
                object value = propertyInfo.GetValue(TT, null);
                switch (propertyInfo.PropertyType.ToString())
                {
                    case "System.DateTime":
                        if ((DateTime)value != DateTime.MinValue && (DateTime)value != DateTime.MaxValue)
                        {
                            if (HelperUtility.IsPrimary(propertyInfo))
                                whereClause.Add(HelperUtility.ColumnName(propertyInfo) +  "='" + value + "'");
                        }
                        break;
                    case "System.String":
                    case "System.Byte":
                    case "System.Char":
                          if (HelperUtility.IsPrimary(propertyInfo))
                               whereClause.Add(HelperUtility.ColumnName(propertyInfo) +  "='" + value + "'");
                        break;
                    default:
                           if (HelperUtility.IsPrimary(propertyInfo))
                             whereClause.Add(HelperUtility.ColumnName(propertyInfo) + "=" + value);
                        break;
                }
                    
            }
            return string.Format("DELETE FROM {0}  WHERE {1}",
                             HelperUtility.GetTableName<T>(),
                             string.Join(" AND ", whereClause.ToArray())
                             );
        }

        
        /// <summary>   Wheres the given expression. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="expression">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Where(Expression<Func<T, bool>> expression)
        {
            _where = _where == null
                ? expression
                : Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(_where.Body, expression.Body),
                    _where.Parameters);
            return this;
        }

        
        /// <summary>   Gets the where. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Where()
        {
            return _where == null ? string.Empty :  string.Format("WHERE {0}", HelperUtility.ConvertExpressionToString(_where.Body));
        }

        
        /// <summary>   Joins. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <typeparam name="J">    . </typeparam>
        /// <param name="left">     . </param>
        /// <param name="right">    . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Join<J>(Expression<Func<T, object>> left, Expression<Func<J, object>> right)
        {
            _join.Add(new KeyValuePair<Type, Tuple<string, string>>(typeof(J),new Tuple<string, string>(HelperUtility.ConvertExpressionToString(left), HelperUtility.ConvertExpressionToString(right))));
            return this;
        }

        
        /// <summary>   Gets the join. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Join()
        {
            return string.Join(" ",
                _join.Select(
                    x =>
                        string.Format("JOIN {0} ON {1}.{2}={3}.{4}", HelperUtility.GetTableName(x.Key) ,  HelperUtility.GetTableName<T>() ,
                         x.Value.Item1 ,
                            HelperUtility.GetTableName(x.Key), x.Value.Item2 )));
        }

        
        /// <summary>   Group by. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="expression">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> GroupBy(params Expression<Func<T, object>>[] expression)
        {
            _groupBy.AddRange(expression.Select(HelperUtility.ConvertExpressionToString));
            return this;
        }

        
        /// <summary>   Group by. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string GroupBy()
        {
            return _groupBy.Any() ? string.Format("GROUP BY {0}", string.Join(",", _groupBy)) : string.Empty;
        }

        
        /// <summary>   Order by. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="expression">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> OrderBy(params Expression<Func<T, object>>[] expression)
        {
            _orderBy.AddRange(expression.Select(HelperUtility.ConvertExpressionToString));
            return this;
        }

        
        /// <summary>   Order by. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string OrderBy()
        {
            return _orderBy.Any() ? string.Format("ORDER BY {0}", string.Join(",", _orderBy)) : string.Format("ORDER BY {0} ", Projections()[0]);
        }

        
        /// <summary>   Limits. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="limit">    . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Limit(int limit)
        {
            _limit = limit;
            return this;
        }

        
        /// <summary>   Gets the limit. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Limit()
        {
            return string.Format("LIMIT {0}", _limit);
        }

        
        /// <summary>   Offsets. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="offset">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Offset(int offset)
        {
            _offset = offset;
            return this;
        }

        
        /// <summary>   Gets the offset. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Offset()
        {
            return string.Format("OFFSET {0}", _offset);
        }

        
        /// <summary>   Sums the given expression. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="expression">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Sum(params Expression<Func<T, object>>[] expression)
        {
            _sum.AddRange(expression.Select(HelperUtility.ConvertExpressionToString));
            return this;
        }

        
        /// <summary>   Gets the sum. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Sum()
        {
            return string.Join(string.Empty, _sum.Select(x => string.Format(",SUM({0}) AS {0}", x)));
        }

        
        /// <summary>   Counts the given expression. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <param name="expression">   . </param>
        ///
        /// <returns>   A QueryBuilder&lt;T&gt; </returns>
        

        public QueryBuilder<T> Count(params Expression<Func<T, object>>[] expression)
        {
            _count.AddRange(expression.Select(HelperUtility.ConvertExpressionToString));
            return this;
        }

        
        /// <summary>   Gets the count. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Count()
        {
            return string.Join(string.Empty, _count.Select(x => string.Format(",COUNT({0}) AS {0}", x)));
        }

        
        /// <summary>   Gets the projections. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        

        public List<string> Projections()
        {
            var properties = _groupBy.Any() ? _groupBy : Properties();
            if (_projection.Any())
            {
                _projection = _projection.Intersect(properties).ToList();
            }
            return _projection.Any() ? _projection : properties;
        }

        
        /// <summary>   Gets the projection. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A string. </returns>
        

        public string Projection()
        {
            return string.Join(",", Projections());
        }

        
        /// <summary>   Gets the properties. </summary>
        ///
        /// <remarks>   Shahid Kochak, 7/20/2017. </remarks>
        ///
        /// <returns>   A List&lt;string&gt; </returns>
        

        private List<string> Properties()
        {
            return Properties(typeof(T));
        }
        List<string> Properties(Type input)
        {
            return (from p in (input.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase | BindingFlags.SetProperty))
                            .Select(prop => new
                            {
                                prop,
                                attr = prop.GetCustomAttributes(typeof(CustomAttribute)).FirstOrDefault()
                            }).Where(y=> y.attr == null || ((CustomAttribute)y.attr).NotMapped == false)
                            .Select(x => new
                            {
                                name = x.attr == null ? x.prop.Name : ((CustomAttribute)x.attr).Name ?? x.prop.Name,
                                x.prop
                            }).Where(x=>x.prop.GetMethod.IsVirtual==false)
                           select p.name).ToList();
        }
    }
}

