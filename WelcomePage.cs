using System;
using System.Windows.Forms;

namespace DeBorja_
{
    public partial class WelcomePage : Form
    {
        private bool loggingOut = false;

        public WelcomePage()
        {
            InitializeComponent();

            
            HookByText(this, "log out", btnLogOut_Click);
            HookByText(this, "logout", btnLogOut_Click);
            HookByText(this, "sign out", btnLogOut_Click);

            
            this.FormClosed += (s, e) =>
            {
                if (!loggingOut) Application.Exit();
            };
        }

        private void HookByText(Control parent, string keyword, EventHandler handler)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.HasChildren)
                {
                    HookByText(c, keyword, handler);
                }

                if (c is Button && c.Text.ToLower().Contains(keyword))
                {
                    c.Click -= handler;
                    c.Click += handler;
                }
            }
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to log out?",
                "Log Out",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                loggingOut = true;

                SignInForm signIn = new SignInForm();
                signIn.Show();
                this.Close();
            }
        }
    }
}