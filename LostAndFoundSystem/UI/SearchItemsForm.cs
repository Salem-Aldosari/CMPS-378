using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using LostAndFoundSystem.Models;
using LostAndFoundSystem.BLL;
using LostAndFoundSystem.DAL;
using LostAndFoundSystem.Utilities;

namespace LostAndFoundSystem.UI
{
    /// <summary>
    /// Form for searching and claiming items
    /// </summary>
    public partial class SearchItemsForm : Form
    {
        private TextBox txtKeyword;
        private ComboBox cmbCategory;
        private ComboBox cmbLocation;
        private ComboBox cmbStatus;
        private Button btnSearch;
        private Button btnClear;
        private DataGridView dgvResults;
        private Button btnClaimItem;
        private Button btnViewDetails;
        
        private ItemService _itemService;
        private CategoryRepository _categoryRepository;
        private LocationRepository _locationRepository;
        private ClaimService _claimService;

        public SearchItemsForm()
        {
            InitializeComponents();
            _itemService = new ItemService();
            _categoryRepository = new CategoryRepository();
            _locationRepository = new LocationRepository();
            _claimService = new ClaimService();
            LoadFilters();
            LoadAllItems();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Search Found Items";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 240, 240);

            int leftMargin = 20;
            int yPos = 20;

            // Keyword search
            AddLabel("Search Keyword:", leftMargin, yPos);
            txtKeyword = new TextBox
            {
                Location = new Point(leftMargin + 120, yPos),
                Size = new Size(250, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtKeyword);

            // Category filter
            AddLabel("Category:", leftMargin, yPos + 40);
            cmbCategory = new ComboBox
            {
                Location = new Point(leftMargin + 120, yPos + 40),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbCategory);

            // Location filter
            AddLabel("Location:", leftMargin + 350, yPos + 40);
            cmbLocation = new ComboBox
            {
                Location = new Point(leftMargin + 440, yPos + 40),
                Size = new Size(200, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbLocation);

            // Status filter
            AddLabel("Status:", leftMargin + 670, yPos + 40);
            cmbStatus = new ComboBox
            {
                Location = new Point(leftMargin + 740, yPos + 40),
                Size = new Size(150, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbStatus.Items.AddRange(new object[] { "All", "Active", "Claimed", "Archived" });
            cmbStatus.SelectedIndex = 0;
            this.Controls.Add(cmbStatus);

            // Search button
            btnSearch = new Button
            {
                Text = "Search",
                Location = new Point(leftMargin + 400, yPos),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnSearch.Click += BtnSearch_Click;
            this.Controls.Add(btnSearch);

            // Clear button
            btnClear = new Button
            {
                Text = "Clear",
                Location = new Point(leftMargin + 520, yPos),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnClear.Click += BtnClear_Click;
            this.Controls.Add(btnClear);

            // Data grid view
            dgvResults = new DataGridView
            {
                Location = new Point(leftMargin, yPos + 110),
                Size = new Size(940, 450),
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false
            };
            this.Controls.Add(dgvResults);

            // View Details button
            btnViewDetails = new Button
            {
                Text = "View Details",
                Location = new Point(leftMargin, yPos + 580),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnViewDetails.Click += BtnViewDetails_Click;
            this.Controls.Add(btnViewDetails);

            // Claim Item button
            btnClaimItem = new Button
            {
                Text = "Claim Item",
                Location = new Point(leftMargin + 180, yPos + 580),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnClaimItem.Click += BtnClaimItem_Click;
            this.Controls.Add(btnClaimItem);
        }

        private void AddLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(110, 20),
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };
            this.Controls.Add(label);
        }

        private void LoadFilters()
        {
            try
            {
                // Load categories
                var categories = _categoryRepository.GetAllCategories();
                categories.Insert(0, new Category { CategoryID = 0, CategoryName = "All" });
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";

                // Load locations
                var locations = _locationRepository.GetAllLocations();
                locations.Insert(0, new Location { LocationID = 0, LocationName = "All" });
                cmbLocation.DataSource = locations;
                cmbLocation.DisplayMember = "LocationName";
                cmbLocation.ValueMember = "LocationID";
            }
            catch (Exception ex)
            {
                ValidationHelper.ShowError($"Error loading filters: {ex.Message}");
            }
        }

        private void LoadAllItems()
        {
            try
            {
                var items = _itemService.GetAllItems();
                DisplayResults(items);
            }
            catch (Exception ex)
            {
                ValidationHelper.ShowError($"Error loading items: {ex.Message}");
            }
        }

        private void DisplayResults(System.Collections.Generic.List<Item> items)
        {
            dgvResults.DataSource = null;
            dgvResults.Rows.Clear();
            dgvResults.Columns.Clear();

            if (items == null || !items.Any())
            {
                MessageBox.Show("No items found matching your search criteria.", 
                    "No Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            dgvResults.Columns.Add("ItemID", "ID");
            dgvResults.Columns.Add("Title", "Title");
            dgvResults.Columns.Add("Category", "Category");
            dgvResults.Columns.Add("Location", "Location");
            dgvResults.Columns.Add("FoundDate", "Found Date");
            dgvResults.Columns.Add("Status", "Status");
            dgvResults.Columns.Add("DaysAgo", "Age");

            dgvResults.Columns["ItemID"].Width = 50;
            dgvResults.Columns["Title"].Width = 200;
            dgvResults.Columns["Category"].Width = 120;
            dgvResults.Columns["Location"].Width = 150;
            dgvResults.Columns["FoundDate"].Width = 150;
            dgvResults.Columns["Status"].Width = 100;
            dgvResults.Columns["DaysAgo"].Width = 100;

            foreach (var item in items)
            {
                int rowIndex = dgvResults.Rows.Add();
                var row = dgvResults.Rows[rowIndex];
                
                row.Cells["ItemID"].Value = item.ItemID;
                row.Cells["Title"].Value = item.Title;
                row.Cells["Category"].Value = item.CategoryName;
                row.Cells["Location"].Value = item.LocationName;
                row.Cells["FoundDate"].Value = item.FoundDate.ToString("MM/dd/yyyy hh:mm tt");
                row.Cells["Status"].Value = item.Status;
                row.Cells["DaysAgo"].Value = ValidationHelper.GetDaysAgo(item.FoundDate);
                
                row.Tag = item;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string keyword = txtKeyword.Text.Trim();
                int? categoryId = cmbCategory.SelectedValue != null && Convert.ToInt32(cmbCategory.SelectedValue) > 0 
                    ? Convert.ToInt32(cmbCategory.SelectedValue) : (int?)null;
                int? locationId = cmbLocation.SelectedValue != null && Convert.ToInt32(cmbLocation.SelectedValue) > 0 
                    ? Convert.ToInt32(cmbLocation.SelectedValue) : (int?)null;
                string status = cmbStatus.SelectedItem?.ToString() != "All" ? cmbStatus.SelectedItem?.ToString() : null;

                var items = _itemService.SearchItems(keyword, categoryId, locationId, status);
                DisplayResults(items);
            }
            catch (Exception ex)
            {
                ValidationHelper.ShowError($"Error searching items: {ex.Message}");
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtKeyword.Clear();
            cmbCategory.SelectedIndex = 0;
            cmbLocation.SelectedIndex = 0;
            cmbStatus.SelectedIndex = 0;
            LoadAllItems();
        }

        private void BtnViewDetails_Click(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count == 0)
            {
                ValidationHelper.ShowWarning("Please select an item to view details.");
                return;
            }

            var item = (Item)dgvResults.SelectedRows[0].Tag;
            
            string details = $"Item Details:\n\n" +
                           $"Title: {item.Title}\n" +
                           $"Category: {item.CategoryName}\n" +
                           $"Location: {item.LocationName}\n" +
                           $"Found Date: {item.FoundDate:MM/dd/yyyy hh:mm tt}\n" +
                           $"Status: {item.Status}\n" +
                           $"Description: {item.Description}\n" +
                           $"Finder Notes: {item.FinderNotes}";

            MessageBox.Show(details, "Item Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClaimItem_Click(object sender, EventArgs e)
        {
            if (dgvResults.SelectedRows.Count == 0)
            {
                ValidationHelper.ShowWarning("Please select an item to claim.");
                return;
            }

            var item = (Item)dgvResults.SelectedRows[0].Tag;

            // Check if item can be claimed
            if (!_claimService.CanClaimItem(item.ItemID, out string errorMsg))
            {
                ValidationHelper.ShowError(errorMsg);
                return;
            }

            // Show claim form (simplified version)
            string claimerName = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter your full name:", "Claim Item", "", -1, -1);

            if (string.IsNullOrWhiteSpace(claimerName))
                return;

            string contactInfo = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter your contact information (email or phone):", "Claim Item", "", -1, -1);

            if (string.IsNullOrWhiteSpace(contactInfo))
                return;

            string itemDescription = Microsoft.VisualBasic.Interaction.InputBox(
                "Describe the item to verify ownership:", "Claim Item", "", -1, -1);

            var claim = new Claim
            {
                ItemID = item.ItemID,
                ClaimerUserID = AuthService.CurrentUser?.UserID ?? 0,
                ClaimerName = claimerName,
                ContactInfo = contactInfo,
                ItemDescription = itemDescription
            };

            if (_claimService.SubmitClaim(claim, out errorMsg))
            {
                ValidationHelper.ShowSuccess("Claim submitted successfully! Staff will review your claim.");
                BtnSearch_Click(null, null); // Refresh results
            }
            else
            {
                ValidationHelper.ShowError(errorMsg);
            }
        }
    }
}
