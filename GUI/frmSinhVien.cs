using BUS;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmSinhVien : Form
    {
        private readonly LopServices LopServices = LopServices.Instance;
        private readonly StudentService studentService = StudentService.Instance;

        private ToolTip toolTip;

        public frmSinhVien()
        {
            InitializeComponent();
           

            // Tạo ToolTip
            toolTip = new ToolTip();
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dataGridView1);
                dateTimePicker1.Format = DateTimePickerFormat.Custom;
                dateTimePicker1.CustomFormat = "dd/MM/yyyy";
                var listLop = LopServices.GetAll();
                var listStudents = studentService.GetAll();
                FillFalcultyCombobox(listLop);
                BindGrid(listStudents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FillFalcultyCombobox(List<Lop> listLop)
        {
            listLop.Insert(0, new Lop());
            this.cmbLopHoc.DataSource = listLop;
            this.cmbLopHoc.DisplayMember = "TenLop";
            this.cmbLopHoc.ValueMember = "MaLop";
        }

        //Hàm binding gridView từ list sinh viên                  
        private void BindGrid(List<SinhVien> listStudent)
        {
            dataGridView1.Rows.Clear();
            foreach (var item in listStudent)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells[0].Value = item.MaSv;
                dataGridView1.Rows[index].Cells[1].Value = item.HoTenSV;
                dataGridView1.Rows[index].Cells[2].Value = item.Ngaysinh;
                dataGridView1.Rows[index].Cells[3].Value = item.Lop.TenLop;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row;
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count) 
                {
                    row = dataGridView1.Rows[e.RowIndex];
                    txtMaSv.Text = row.Cells["clmMaSv"].Value.ToString();
                    txtHoTen.Text = row.Cells["clmHoTen"].Value.ToString();
                    dateTimePicker1.Text = row.Cells["clmNgaySinh"].Value.ToString();
                    cmbLopHoc.Text = row.Cells["clmLop"].Value.ToString();
                }
            }
            catch (Exception ex) { }


            //bật buttun
            //btnLuu.Enabled = true;
            //btnKhongLuu.Enabled = true;
        }

        public void setGridViewStyle(DataGridView dgview)
        {
            dgview.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgview.RowHeadersVisible = false;
            dgview.ReadOnly = true;
            clmNgaySinh.DefaultCellStyle.Format = "dd/MM/yyyy"; 
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (checkDataInput())
            {
                SinhVien s = studentService.FindById(txtMaSv.Text);

                if (s == null)
                {
                    // Thêm mới sinh viên
                    s = new SinhVien();
                    s.MaSv = txtMaSv.Text;
                    s.HoTenSV = txtHoTen.Text;
                    s.Ngaysinh = dateTimePicker1.Value;
                    s.MaLop = (string)cmbLopHoc.SelectedValue;
                    studentService.InsertUpdate(s);
                    BindGrid(studentService.GetAll());
                    MessageBox.Show("Thêm dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLuu.Enabled = true;
                    btnKhongLuu.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Sinh viên đã tồn tại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

            };
        }

        private bool checkDataInput()
        {
            if (string.IsNullOrEmpty(txtMaSv.Text.ToString()))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else

            if (string.IsNullOrEmpty(txtHoTen.Text.ToString()))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else
            {
                if ((txtMaSv.Text.ToString().Length != 6))
                {
                    MessageBox.Show("Mã số sinh viên phải có 10 kí tự số!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    if (String.IsNullOrEmpty(cmbLopHoc.Text.ToString()))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (checkDataInput())
            {
                SinhVien dbDelete = studentService.FindById(txtMaSv.Text);
                if (dbDelete != null)
                {
                    if (MessageBox.Show("Xác nhận xóa", "thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        //studentService.Remove(dbDelete);
                        studentService.RemoveByID(dbDelete.MaSv);
                    }
                }

                BindGrid(studentService.GetAll());
                MessageBox.Show("Xóa thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnLuu.Enabled = true;
                btnKhongLuu.Enabled = true;
            }
           
        }

        private void bthnSua_Click(object sender, EventArgs e)
        {
            if (checkDataInput())
            {
                SinhVien s = studentService.FindById(txtMaSv.Text);

                if (s == null)
                {
                    MessageBox.Show("Không thể sữa MSSV!", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                { 
                    s.MaSv = txtMaSv.Text;
                    s.HoTenSV = txtHoTen.Text;
                    s.Ngaysinh = dateTimePicker1.Value;
                    s.MaLop = (String)cmbLopHoc.SelectedValue;
                    studentService.InsertUpdate(s);
                    BindGrid(studentService.GetAll());
                    MessageBox.Show("Cập nhật dữ liệu thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLuu.Enabled = true;
                    btnKhongLuu.Enabled = true;
                }

            }
            
        }

        private void dataGridView1_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            
        }

        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                // Ném ngoại lệ tùy chỉnh hoặc xử lý logic của bạn ở đây
                throw new InvalidOperationException("Click vào hàng header không được phép.");
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi hoặc xử lý ngoại lệ
                MessageBox.Show(ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }

        private void frmSinhVien_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void frmSinhVien_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Hiển thị hộp thoại xác nhận
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đóng Form này không?",
                                                  "Xác nhận đóng Form",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

            // Kiểm tra phản hồi của người dùng
            if (result == DialogResult.No)
            {
                // Hủy bỏ việc đóng Form
                e.Cancel = true;
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {
            SinhVien sv = studentService.FindByName(edtSeach.Text.Trim());
            if (sv == null)
            {
                MessageBox.Show("Không tìm thấy tên sinh viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                txtMaSv.Text = sv.MaSv;
                txtHoTen.Text = sv.HoTenSV ;
                dateTimePicker1.Text = sv.Ngaysinh.ToString();
                cmbLopHoc.Text = LopServices.FindByID(sv.MaLop).TenLop;
                seachIndex(sv.MaSv);

                toolTip.Show("Đã tìm thấy.", this, 300, 500, 3000);
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void btnKhongLuu_Click(object sender, EventArgs e)
        {
            btnLuu.Enabled = false;
            btnKhongLuu.Enabled = false;
        }

        private void seachIndex(String valuess)
        {
            string searchValue = valuess; // Giá trị cần tìm kiếm
            int columnIndex = 0; // Cột chứa giá trị cần tìm kiếm
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[columnIndex].Value != null && row.Cells[columnIndex].Value.ToString().Equals(searchValue))
                {
                    int rowIndex = row.Index;
                    dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[columnIndex];
                    break;
                }
            }
        }

    }
}
