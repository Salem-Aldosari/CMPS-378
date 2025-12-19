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
    /// Form for adding new found items
    /// </summary>
    public partial class AddItemForm : Form
    {
        private TextBox txtTitle;
        private ComboBox cmbCategory;
        private ComboBox cmbLocation;
        private DateTimePicker dtpFoundDate;
        private TextBox txtDescription;
        private TextBox txtFinderNotes;
        private Button btnSave;
        private Button btnCancel;
        
        private ItemService _itemService;
        private CategoryRepository _categoryRepository;
        private LocationRepository _locationRepository;

        public AddItemForm()
        {
            InitializeComponents();
            _itemService = new ItemService();
            _categoryRepository = new CategoryRepository();
            _locationRepository = new LocationRepository();
            LoadCategories();
            LoadLocations();
        }

        private void InitializeComponents()
        {
            // Form settings
            this.Text = "Add Found Item";
            this.Size = new Size(600, 650);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 240, 240);

            int yPos = 20;
            int labelWidth = 120;
            int controlWidth = 400;
            int leftMargin = 50;

            // Title
            AddLabel("Item Title:*", leftMargin, yPos);
            txtTitle = new TextBox
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10)
            };
            this.Controls.Add(txtTitle);
            yPos += 65;

            // Category
            AddLabel("Category:*", leftMargin, yPos);
            cmbCategory = new ComboBox
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbCategory);
            yPos += 65;

            // Location
            AddLabel("Found Location:*", leftMargin, yPos);
            cmbLocation = new ComboBox
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            this.Controls.Add(cmbLocation);
            yPos += 65;

            // Found Date
            AddLabel("Found Date:*", leftMargin, yPos);
            dtpFoundDate = new DateTimePicker
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 25),
                Font = new Font("Segoe UI", 10),
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "MM/dd/yyyy hh:mm tt",
                Value = DateTime.Now
            };
            this.Controls.Add(dtpFoundDate);
            yPos += 65;

            // Description
            AddLabel("Description:", leftMargin, yPos);
            txtDescription = new TextBox
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 80),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtDescription);
            yPos += 120;

            // Finder Notes
            AddLabel("Finder Notes:", leftMargin, yPos);
            txtFinderNotes = new TextBox
            {
                Location = new Point(leftMargin, yPos + 25),
                Size = new Size(controlWidth, 60),
                Font = new Font("Segoe UI", 10),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };
            this.Controls.Add(txtFinderNotes);
            yPos += 100;

            // Buttons
            btnSave = new Button
            {
                Text = "Save Item",
                Location = new Point(leftMargin, yPos),
                Size = new Size(180, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;
            this.Controls.Add(btnSave);

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(leftMargin + 200, yPos),
                Size = new Size(180, 40),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(108, 117, 125),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => this.Close();
            this.Controls.Add(btnCancel);
        }

        private void AddLabel(string text, int x, int y)
        {
            var label = new Label
            {
                Text = text,
                Location = new Point(x, y),
                Size = new Size(400, 20),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            this.Controls.Add(label);
        }

        private void LoadCategories()
        {
            try
            {
                var categories = _categoryRepository.GetAllCategories();
                cmbCategory.DataSource = categories;
                cmbCategory.DisplayMember = "CategoryName";
                cmbCategory.ValueMember = "CategoryID";
                cmbCategory.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ValidationHelper.ShowError($"Error loading categories: {ex.Message}");
            }
        }

        private void LoadLocations()
        {
            try
            {
                var locations = _locationRepository.GetAllLocations();
                cmbLocation.DataSource = locations;
                cmbLocation.DisplayMember = "LocationName";
                cmbLocation.ValueMember = "LocationID";
                cmbLocation.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                ValidationHelper.ShowError($"Error loading locations: {ex.Message}");
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (!ValidationHelper.IsRequired(txtTitle, "Title", out string errorMsg))
            {
                ValidationHelper.ShowError(errorMsg);
                return;
            }

            if (!ValidationHelper.IsSelected(cmbCategory, "category", out errorMsg))
            {
                ValidationHelper.ShowError(errorMsg);
                return;
            }

            if (!ValidationHelper.IsSelected(cmbLocation, "location", out errorMsg))
            {
                ValidationHelper.ShowError(errorMsg);
                return;
            }

            // Create item object
            Item newItem = new Item
            {
                Title = txtTitle.Text.Trim(),
                CategoryID = Convert.ToInt32(cmbCategory.SelectedValue),
                Description = txtDescription.Text.Trim(),
                FoundDate = dtpFoundDate.Value,
                FoundLocationID = Convert.ToInt32(cmbLocation.SelectedValue),
                FinderNotes = txtFinderNotes.Text.Trim(),
                LoggedByUserID = AuthService.CurrentUser.UserID,
                Status = "Active"
            };

            // Add item (includes duplicate checking)
            if (_itemService.AddItem(newItem, out string addErrorMsg, out var duplicates))
            {
                // Check for duplicates
                if (duplicates != null && duplicates.Any())
                {
                    string dupMessage = "Warning: The following similar items were found in the last 7 days:\n\n";
                    foreach (var dup in duplicates)
                    {
                        dupMessage += $"• {dup.Title} - {dup.CategoryName} - Found on {dup.FoundDate.ToShortDateString()}\n";
                    }
                    dupMessage += "\nYour item has been saved, but please verify it's not a duplicate.";
                    
                    MessageBox.Show(dupMessage, "Potential Duplicates Found", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    ValidationHelper.ShowSuccess("Item added successfully!");
                }

                this.Close();
            }
            else
            {
                ValidationHelper.ShowError(addErrorMsg);
            }
        }
    }
}
