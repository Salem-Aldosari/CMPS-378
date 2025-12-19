using System;
using System.Drawing;
using System.Windows.Forms;
using LostAndFoundSystem.BLL;

namespace LostAndFoundSystem.UI
{
    /// <summary>
    /// Main dashboard form after login
    /// </summary>
    public partial class DashboardForm : Form
    {
        private Label lblWelcome;
        private Button btnAddItem;
        private Button btnSearchItems;
        private Button btnViewClaims;
        private Button btnAdminPanel;
        private Button btnLogout;
        private Panel panelStats;
        private Label lblStats;
        private ItemService _itemService;
        private ClaimService _claimService;

        public DashboardForm()
        {
            InitializeComponents();
            _itemService = new ItemService();
            _claimService = new ClaimService();
            LoadStatistics();
            SetupRoleBasedAccess();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Campus Lost & Found - Dashboard";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            // Welcome label
            lblWelcome = new Label
            {
                Text = $"Welcome, {AuthService.CurrentUser.Username}!",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                Location = new Point(30, 20),
                Size = new Size(700, 40),
                ForeColor = Color.FromArgb(0, 122, 204)
            };

            // Statistics panel
            panelStats = new Panel
            {
                Location = new Point(30, 80),
                Size = new Size(740, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            lblStats = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(700, 110),
                Font = new Font("Segoe UI", 11),
                Text = "Loading statistics..."
            };
            panelStats.Controls.Add(lblStats);

            // Add Item button
            btnAddItem = CreateButton("Add Found Item", new Point(30, 260), Color.FromArgb(40, 167, 69));
            btnAddItem.Click += BtnAddItem_Click;

            // Search Items button
            btnSearchItems = CreateButton("Search Items", new Point(250, 260), Color.FromArgb(0, 122, 204));
            btnSearchItems.Click += BtnSearchItems_Click;

            // View Claims button
            btnViewClaims = CreateButton("View Claims", new Point(470, 260), Color.FromArgb(255, 193, 7));
            btnViewClaims.Click += BtnViewClaims_Click;

            // Admin Panel button
            btnAdminPanel = CreateButton("Admin Panel", new Point(30, 340), Color.FromArgb(108, 117, 125));
            btnAdminPanel.Click += BtnAdminPanel_Click;

            // Logout button
            btnLogout = CreateButton("Logout", new Point(250, 340), Color.FromArgb(220, 53, 69));
            btnLogout.Click += BtnLogout_Click;

            // Add controls to form
            this.Controls.Add(lblWelcome);
            this.Controls.Add(panelStats);
            this.Controls.Add(btnAddItem);
            this.Controls.Add(btnSearchItems);
            this.Controls.Add(btnViewClaims);
            this.Controls.Add(btnAdminPanel);
            this.Controls.Add(btnLogout);
        }

        private Button CreateButton(string text, Point location, Color backColor)
        {
            return new Button
            {
                Text = text,
                Location = location,
                Size = new Size(200, 50),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = backColor,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
        }

        private void LoadStatistics()
        {
            try
            {
                var itemStats = _itemService.GetItemStatistics();
                var claimStats = _claimService.GetClaimStatistics();

                int activeItems = itemStats.ContainsKey("Active") ? itemStats["Active"] : 0;
                int claimedItems = itemStats.ContainsKey("Claimed") ? itemStats["Claimed"] : 0;
                int pendingClaims = claimStats.ContainsKey("Pending") ? claimStats["Pending"] : 0;

                lblStats.Text = $"System Statistics:\n\n" +
                               $"• Active Items: {activeItems}\n" +
                               $"• Claimed Items: {claimedItems}\n" +
                               $"• Pending Claims: {pendingClaims}\n" +
                               $"• Your Role: {AuthService.GetCurrentUserRole()}";
            }
            catch (Exception ex)
            {
                lblStats.Text = $"Error loading statistics: {ex.Message}";
            }
        }

        private void SetupRoleBasedAccess()
        {
            // Hide admin panel for non-admin users
            if (!AuthService.IsAdmin())
            {
                btnAdminPanel.Visible = false;
            }

            // Hide Add Item button for students
            if (!AuthService.IsStaff())
            {
                btnAddItem.Visible = false;
            }

            // Hide View Claims button for students
            if (!AuthService.IsStaff())
            {
                btnViewClaims.Visible = false;
            }
        }

        private void BtnAddItem_Click(object sender, EventArgs e)
        {
            AddItemForm form = new AddItemForm();
            form.ShowDialog();
            LoadStatistics(); // Refresh statistics after closing
        }

        private void BtnSearchItems_Click(object sender, EventArgs e)
        {
            SearchItemsForm form = new SearchItemsForm();
            form.ShowDialog();
            LoadStatistics(); // Refresh statistics after closing
        }

        private void BtnViewClaims_Click(object sender, EventArgs e)
        {
            MessageBox.Show("View Claims feature - Staff can review and approve/reject pending claims here.",
                "View Claims",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            // In a full implementation, this would open a ViewClaimsForm
        }

        private void BtnAdminPanel_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Admin Panel - Manage categories, locations, and users here.",
                "Admin Panel",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            // In a full implementation, this would open an AdminPanelForm
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?",
                "Confirm Logout",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                AuthService authService = new AuthService();
                authService.Logout();
                
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.FormClosed += (s, args) => this.Close();
                loginForm.Show();
            }
        }
    }
}
