using System;
using System.Collections.Generic;
using System.Text;

namespace Jabber.Net.Server.Data.Sql
{
    public class SqlInsert : ISqlInstruction
    {
        private readonly List<string> columns = new List<string>();
        private readonly string table;
        private readonly List<object> values = new List<object>();
        private int identityPosition = -1;
        private object nullValue;
        private SqlQuery query;
        private bool replaceExists;
        private bool ignoreExists;
        private bool returnIdentity;


        public SqlInsert(string table)
            : this(table, false)
        {
        }

        public SqlInsert(string table, bool replaceExists)
        {
            this.table = table;
            ReplaceExists(replaceExists);
        }


        public string ToString(ISqlDialect dialect)
        {
            var sql = new StringBuilder();

            if (ignoreExists)
            {
                sql.Append(dialect.InsertIgnore);
            }
            else
            {
                sql.Append(replaceExists ? "replace" : "insert");
            }
            sql.AppendFormat(" into {0}", table);
            bool identityInsert = IsIdentityInsert();
            if (0 < columns.Count)
            {
                sql.Append("(");
                for (int i = 0; i < columns.Count; i++)
                {
                    if (identityInsert && identityPosition == i) continue;
                    sql.AppendFormat("{0}, ", columns[i]);
                }
                sql.Remove(sql.Length - 2, 2).Append(")");
            }
            if (query != null)
            {
                sql.AppendFormat(" {0}", query.ToString(dialect));
                return sql.ToString();
            }
            sql.Append(" values (");
            for (int i = 0; i < values.Count; i++)
            {
                if (identityInsert && identityPosition == i) continue;
                sql.Append("?, ");
            }
            sql.Remove(sql.Length - 2, 2).Append(")");
            if (returnIdentity)
            {
                sql.AppendFormat("; select {0}", identityInsert ? dialect.IdentityQuery : "?");
            }
            return sql.ToString();
        }

        public object[] GetParameters()
        {
            if (query != null)
            {
                return query.GetParameters();
            }
            var copy = new List<object>(values);
            if (IsIdentityInsert())
            {
                copy.RemoveAt(identityPosition);
            }
            else if (returnIdentity)
            {
                copy.Add(copy[identityPosition]);
            }
            return copy.ToArray();
        }


        public SqlInsert InColumns(params string[] columns)
        {
            this.columns.AddRange(columns);
            return this;
        }

        public SqlInsert Values(params object[] values)
        {
            this.values.AddRange(values);
            return this;
        }

        public SqlInsert Values(SqlQuery query)
        {
            this.query = query;
            return this;
        }

        public SqlInsert InColumnValue(string column, object value)
        {
            return InColumns(column).Values(value);
        }

        public SqlInsert ReplaceExists(bool replaceExists)
        {
            this.replaceExists = replaceExists;
            return this;
        }

        public SqlInsert IgnoreExists(bool ignoreExists)
        {
            this.ignoreExists = ignoreExists;
            return this;
        }

        public SqlInsert Identity<TIdentity>(int position, TIdentity nullValue)
        {
            return Identity(position, nullValue, false);
        }

        public SqlInsert Identity<TIdentity>(int position, TIdentity nullValue, bool returnIdentity)
        {
            identityPosition = position;
            this.nullValue = nullValue;
            this.returnIdentity = returnIdentity;
            return this;
        }

        public override string ToString()
        {
            return ToString(SqlDialect.Default);
        }

        private bool IsIdentityInsert()
        {
            if (identityPosition < 0) return false;
            if (values[identityPosition] != null && nullValue != null &&
                values[identityPosition].GetType() != nullValue.GetType())
            {
                throw new InvalidCastException(string.Format("Identity null value must be {0} type.",
                                                             values[identityPosition].GetType()));
            }
            return Equals(values[identityPosition], nullValue);
        }
    }
}