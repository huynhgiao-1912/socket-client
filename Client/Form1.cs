
using System;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;;

namespace Client
{
    public partial class Form1 : Form
    {
        StreamReader reader = null;  // đọc dũ liệu
        StreamWriter writer = null;   // ghi dữ liệu
        TcpClient client = null;
        public Form1()
        {
            InitializeComponent();
        }
        public void Load() // hiển thị trạng thái thao tác đang làm
        {
            if (client != null)
            {
                string s = reader.ReadLine();
                txtInfo.Text += s + Environment.NewLine;// newline: xuống dòng
            }
            
        }
        int rown = 0; // số dòng trong bảng hiển thị data search
        public void AddDataListView()
        {
            if (client != null )
            {
                string s = reader.ReadLine(); // đọc data gởi từ server
                if (s != "")
                {
                    if (rown == 0) // nếu là dòng đầu tiên thì show data
                    {
                        string[] data = s.Split('/'); //  tách chuỗi bởi dấu /
                        ListViewItem item = new ListViewItem(data[0]); // lấy giá trị ầu trong chuỗi vùa tách
                        item.SubItems.Add(data[1]);
                        item.SubItems.Add(data[2]);
                        item.SubItems.Add(data[3]);
                        item.SubItems.Add(data[4]);
                        lstDanhSach.Items.Add(item);// add vào table
                        rown++;
                    }
                    else // nếu ko phải dòng ầu tiên , xóa data dòng đầu rồi mới showdata mới
                    {
                        lstDanhSach.Items.RemoveAt(0);
                        string[] data = s.Split('/');
                        ListViewItem item = new ListViewItem(data[0]);
                        item.SubItems.Add(data[1]);
                        item.SubItems.Add(data[2]);
                        item.SubItems.Add(data[3]);
                        item.SubItems.Add(data[4]);
                        lstDanhSach.Items.Add(item);
                    }
                }
                else
                {
                    txtInfo.Text += "Khong tim kiem thay" + Environment.NewLine;
                    if(rown !=0)
                    {
                        lstDanhSach.Items.RemoveAt(0);// xóa data dòng đầu trong table
                    }    
                }
            }

        }


        private void btnConnect_Click(object sender, EventArgs e) // connect server
        {
            try
            {
                string ip = txtIP.Text.ToString();// lấy data từ textbox
                int port = int.Parse(txtPort.Text);
                client = new TcpClient(ip, port);
                reader = new StreamReader(client.GetStream());// set up để đọc data
                writer = new StreamWriter(client.GetStream());// set up để gởi data
                if (client.Connected == true)
                {
                    txtInfo.Text = "Ket noi thanh cong" + Environment.NewLine;
                }
            }
            catch(Exception ex)
            {
                txtInfo.Text = "Ket noi that bai !!!" + Environment.NewLine;
            }
           
        }


        int checkLogin = 0; // kiểm tra đăng nhập 
        private void btnLogin_Click(object sender, EventArgs e) // login
        {
            if (client != null)// ktra client đã kết nối
            {
                checkLogin = 1; // đã đăng nhập thành công
                writer.WriteLine(("login")); // gỏi data qua server báo là client đang login
                string u = txtUsername.Text;
                string p = txtPassword.Text;
                writer.WriteLine(u);// gởi user qua server
                writer.WriteLine(p);// send pass
                writer.Flush();// send
                txtInfo.Text += u + Environment.NewLine;
                txtInfo.Text += p + Environment.NewLine;
                Load();//hàm show trạng thái đang thực hiện , kết quả
            }
            else
            {
                txtInfo.Text += "Chua ket noi voi server" + Environment.NewLine;
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)// register
        {
            // tương tự như login
            if (client != null)
            {
                writer.WriteLine(("register"));
                string u = txtUsername.Text;
                string p = txtPassword.Text;
                writer.WriteLine(u);
                writer.WriteLine(p);
                writer.Flush();
                txtInfo.Text += "Username: " + u + Environment.NewLine;
                txtInfo.Text += "Password: " + p + Environment.NewLine;
                Load();
            }
            else
            {
                txtInfo.Text += "Chua ket noi voi server" + Environment.NewLine;
            }
        }
        // tương tự login
        private void btnCancel_Click(object sender, EventArgs e)// hủy kết nối
        {
            if (client != null)// ktra client đã kết nối
            {
                writer.WriteLine("Exit");
                writer.Flush();
                txtInfo.Text += "Huy ket noi " + Environment.NewLine;
                client = null;
                reader = null;
                writer = null;
            }
            else
            {
                txtInfo.Text += "Chua ket noi voi server" + Environment.NewLine;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)// tìm kiếm
        {
            // test:    data: USD,HKD
            if (client != null)// ktra client đã kết nối
            {
                if (checkLogin == 1)// login mới search dc
                {
                    if (txtSearch.Text.ToString() != " ") // search rỗng
                    {
                        writer.WriteLine(("search"));
                        writer.WriteLine(txtSearch.Text.ToString());
                        writer.Flush();
                        txtInfo.Text += "Tim kiem voi tu khoa: " + txtSearch.Text.ToString() + Environment.NewLine;
                        AddDataListView(); // hàm add data table
                    }
                    else
                    {
                        txtInfo.Text += "Chua nhap tu khoa tim kiem" + Environment.NewLine;
                    }
                }
                else
                {
                    txtInfo.Text += "Chua dang nhap" + Environment.NewLine;
                }
            }
            else
            {
                txtInfo.Text += "Chua ket noi voi server" + Environment.NewLine;
            }    

        }

       
    }
}
