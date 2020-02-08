using System;

namespace Blacksmith.Sql.Queries
{
    public class Pagination
    {
        private int page;
        private int size;

        public Pagination()
        {
            this.page = 0;
            this.size = int.MaxValue;
        }

        public int Page
        {
            get => this.page;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(this.Page));

                this.page = value;
            }
        }
        public int Size
        {
            get => this.size;
            set
            {
                if(value <= 0)
                    throw new ArgumentOutOfRangeException(nameof(this.Size));

                this.size = value;
            }
        }
    }
}