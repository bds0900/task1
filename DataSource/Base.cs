using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSource
{
    public abstract class Base
    {
        private string _connectionString;

        protected Base(string _connectionString)
        {
            this._connectionString = _connectionString;
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }
    }
}
