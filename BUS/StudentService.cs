using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class StudentService
    {
        private static StudentService instance;
        private static StudentDB context = new StudentDB ();
        private StudentService() { }

        public static StudentService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new StudentService();
                }
                return instance;
            }
        }

        public List<SinhVien> GetAll()
        {
            return context.SinhViens.ToList();
        }


        public SinhVien FindById(string studentId)
        {
            return context.SinhViens.FirstOrDefault(p => p.MaSv == studentId);
        }

        public SinhVien FindByName(string studentName)
        {
            return context.SinhViens.FirstOrDefault(p => p.HoTenSV.Equals(studentName));
        }

        public void InsertUpdate(SinhVien s)
        {
            
            context.SinhViens.AddOrUpdate(s);
            context.SaveChanges();
        }

        public bool RemoveByID(String ID)
        {
            SinhVien stuDele = FindById(ID);
            if (stuDele != null)
            {
                context.SinhViens.Remove(stuDele);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public void Remove(SinhVien s)
        {
            context.SinhViens.Remove(s);
            context.SaveChanges();
        }
    }
}
