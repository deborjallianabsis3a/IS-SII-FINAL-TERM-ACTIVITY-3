using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace DeBorja_
{
    public partial class SignInForm : Form
    {
        public SignInForm()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;

            
            HookByText(this, "sign in", btnSignin_Click, true);
            HookByText(this, "register", linkRegister_Click, false);

            this.FormClosed += (s, e) => Application.Exit();
        }

        private void HookByText(Control parent, string keyword, EventHandler handler, bool buttonOnly)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren)
                {
                    HookByText(c, keyword, handler, buttonOnly);
                    continue;
                }

                bool typeOk = buttonOnly ? (c is Button) : !(c is Button);
                if (typeOk && c.Text.ToLower().Contains(keyword))
                {
                    c.Click -= handler;
                    c.Click += handler;
                    c.Cursor = Cursors.Hand;
                }
            }
        }

        private void linkRegister_Click(object sender, EventArgs e)
        {
            CreateYourAccount register = new CreateYourAccount();
            register.Show();
            this.Hide();
        }

        private void btnSignin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            if (username == "" || password == "")
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string storedHash;
                string fullname;

                using (OleDbConnection con = Database.GetConnection())
                {
                    con.Open();
                    using (OleDbCommand cmd = new OleDbCommand("SELECT * FROM [Users] WHERE [Username] = ?", con))
                    {
                        cmd.Parameters.AddWithValue("@Username", username);
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            storedHash = reader["Password"].ToString();
                            fullname = reader["FullName"].ToString();
                        }
                    }
                }

                if (!PasswordHelper.VerifyPassword(password, storedHash))
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show("Welcome, " + fullname + "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WelcomePage welcome = new WelcomePage();
                welcome.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while signing in: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void textBox3_TextChanged(object sender, EventArgs e) { }
        private void textBox4_TextChanged(object sender, EventArgs e) { }
        private void SignInForm_Load(object sender, EventArgs e) { }
    }
}