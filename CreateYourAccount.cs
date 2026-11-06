using System;
using System.Data.OleDb;
using System.Windows.Forms;

namespace DeBorja_
{
    public partial class CreateYourAccount : Form
    {
        public CreateYourAccount()
        {
            InitializeComponent();

            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPasssword.UseSystemPasswordChar = true;

            
            HookByText(this, "register", btnRegister_Click, true);
            HookByText(this, "sign in", linkSignIn_Click, false);

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

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string fullname = txtFullName.Text.Trim();
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPasssword.Text;

            if (fullname == "" || username == "" || password == "" || confirmPassword == "")
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (password.Length < 8)
            {
                MessageBox.Show("Password must be at least 8 characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = Database.GetConnection())
                {
                    con.Open();

                    using (OleDbCommand checkCmd = new OleDbCommand("SELECT COUNT(*) FROM [Users] WHERE [Username] = ?", con))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        if (Convert.ToInt32(checkCmd.ExecuteScalar()) > 0)
                        {
                            MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    using (OleDbCommand cmd = new OleDbCommand("INSERT INTO [Users] ([FullName], [Username], [Password]) VALUES (?, ?, ?)", con))
                    {
                        cmd.Parameters.AddWithValue("@fullname", fullname);
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", PasswordHelper.HashPassword(password));

                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            SignInForm loginForm = new SignInForm();
                            loginForm.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Registration failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while registering: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkSignIn_Click(object sender, EventArgs e)
        {
            SignInForm login = new SignInForm();
            login.Show();
            this.Hide();
        }

        
        private void Name_Click(object sender, EventArgs e) { }
        private void textBox5_TextChanged(object sender, EventArgs e) { }
    }
}