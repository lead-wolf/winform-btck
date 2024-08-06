using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LopServices
    {
        private static LopServices instance;
        private static StudentDB context = new StudentDB();
        private LopServices() { }

        public static LopServices Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LopServices();
                }
                return instance;
            }
        }

        public List<Lop> GetAll()
        {
            return context.Lops.ToList();
        }

        public Lop FindByID(string id)
        {
            return context.Lops.FirstOrDefault(p => p.MaLop == id);
        }
    }
}
