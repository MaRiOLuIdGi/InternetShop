namespace InternetShop
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelMain = new System.Windows.Forms.Panel();
            this.dataGridViewProducts = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.groupBoxShopInfo = new System.Windows.Forms.GroupBox();
            this.lblTotalValue = new System.Windows.Forms.Label();
            this.lblDiscountedCount = new System.Windows.Forms.Label();
            this.lblProductCount = new System.Windows.Forms.Label();
            this.lblShopName = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.groupBoxActions = new System.Windows.Forms.GroupBox();
            this.btnAddProduct = new System.Windows.Forms.Button();
            this.btnLoadFromFile = new System.Windows.Forms.Button();
            this.btnSortDesc = new System.Windows.Forms.Button();
            this.btnSortAsc = new System.Windows.Forms.Button();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.btnAddWithoutDiscount = new System.Windows.Forms.Button();
            this.btnAddWithDiscount = new System.Windows.Forms.Button();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.btnSaveToCsv = new System.Windows.Forms.Button();
            this.btnFindCheapest = new System.Windows.Forms.Button();
            this.btnDeleteProduct = new System.Windows.Forms.Button();
            this.btnEditProduct = new System.Windows.Forms.Button();
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.groupBoxShopInfo.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.groupBoxActions.SuspendLayout();
            this.groupBoxSearch.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.dataGridViewProducts);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(250, 50);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(674, 430);
            this.panelMain.TabIndex = 6;
            // 
            // dataGridViewProducts
            // 
            this.dataGridViewProducts.AllowUserToAddRows = false;
            this.dataGridViewProducts.AllowUserToDeleteRows = false;
            this.dataGridViewProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewProducts.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridViewProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewProducts.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewProducts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewProducts.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewProducts.Margin = new System.Windows.Forms.Padding(10);
            this.dataGridViewProducts.Name = "dataGridViewProducts";
            this.dataGridViewProducts.ReadOnly = true;
            this.dataGridViewProducts.RowHeadersWidth = 51;
            this.dataGridViewProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewProducts.Size = new System.Drawing.Size(674, 430);
            this.dataGridViewProducts.TabIndex = 1;
            this.dataGridViewProducts.SelectionChanged += new System.EventHandler(this.dataGridViewProducts_SelectionChanged);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(924, 50);
            this.panelTop.TabIndex = 1;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelTitle.Location = new System.Drawing.Point(12, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(233, 24);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Учет товаров магазина";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.SystemColors.Control;
            this.panelLeft.Controls.Add(this.groupBoxShopInfo);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 50);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(250, 430);
            this.panelLeft.TabIndex = 2;
            // 
            // groupBoxShopInfo
            // 
            this.groupBoxShopInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxShopInfo.Controls.Add(this.lblTotalValue);
            this.groupBoxShopInfo.Controls.Add(this.lblDiscountedCount);
            this.groupBoxShopInfo.Controls.Add(this.lblProductCount);
            this.groupBoxShopInfo.Controls.Add(this.lblShopName);
            this.groupBoxShopInfo.Location = new System.Drawing.Point(12, 12);
            this.groupBoxShopInfo.Name = "groupBoxShopInfo";
            this.groupBoxShopInfo.Size = new System.Drawing.Size(226, 150);
            this.groupBoxShopInfo.TabIndex = 0;
            this.groupBoxShopInfo.TabStop = false;
            this.groupBoxShopInfo.Text = "Информация о магазине";
            // 
            // lblTotalValue
            // 
            this.lblTotalValue.AutoSize = true;
            this.lblTotalValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTotalValue.Location = new System.Drawing.Point(10, 120);
            this.lblTotalValue.Name = "lblTotalValue";
            this.lblTotalValue.Size = new System.Drawing.Size(125, 15);
            this.lblTotalValue.TabIndex = 3;
            this.lblTotalValue.Text = "Общая стоимость: ";
            // 
            // lblDiscountedCount
            // 
            this.lblDiscountedCount.AutoSize = true;
            this.lblDiscountedCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblDiscountedCount.Location = new System.Drawing.Point(10, 90);
            this.lblDiscountedCount.Name = "lblDiscountedCount";
            this.lblDiscountedCount.Size = new System.Drawing.Size(97, 15);
            this.lblDiscountedCount.TabIndex = 2;
            this.lblDiscountedCount.Text = "Со скидкой: 0";
            // 
            // lblProductCount
            // 
            this.lblProductCount.AutoSize = true;
            this.lblProductCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblProductCount.Location = new System.Drawing.Point(10, 60);
            this.lblProductCount.Name = "lblProductCount";
            this.lblProductCount.Size = new System.Drawing.Size(68, 15);
            this.lblProductCount.TabIndex = 1;
            this.lblProductCount.Text = "Товаров: 0";
            // 
            // lblShopName
            // 
            this.lblShopName.AutoSize = true;
            this.lblShopName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblShopName.Location = new System.Drawing.Point(10, 30);
            this.lblShopName.Name = "lblShopName";
            this.lblShopName.Size = new System.Drawing.Size(161, 17);
            this.lblShopName.TabIndex = 0;
            this.lblShopName.Text = "Интернет-магазин";
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.SystemColors.Control;
            this.panelRight.Controls.Add(this.groupBoxActions);
            this.panelRight.Controls.Add(this.groupBoxSearch);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(674, 50);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(250, 430);
            this.panelRight.TabIndex = 4;
            // 
            // groupBoxActions
            // 
            this.groupBoxActions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxActions.Controls.Add(this.btnAddProduct);
            this.groupBoxActions.Controls.Add(this.btnLoadFromFile);
            this.groupBoxActions.Controls.Add(this.btnSortDesc);
            this.groupBoxActions.Controls.Add(this.btnSortAsc);
            this.groupBoxActions.Controls.Add(this.btnOpenFile);
            this.groupBoxActions.Controls.Add(this.btnAddWithoutDiscount);
            this.groupBoxActions.Controls.Add(this.btnAddWithDiscount);
            this.groupBoxActions.Controls.Add(this.btnStatistics);
            this.groupBoxActions.Controls.Add(this.btnSaveToCsv);
            this.groupBoxActions.Controls.Add(this.btnFindCheapest);
            this.groupBoxActions.Controls.Add(this.btnDeleteProduct);
            this.groupBoxActions.Controls.Add(this.btnEditProduct);
            this.groupBoxActions.Location = new System.Drawing.Point(12, 180);
            this.groupBoxActions.Name = "groupBoxActions";
            this.groupBoxActions.Size = new System.Drawing.Size(226, 240);
            this.groupBoxActions.TabIndex = 1;
            this.groupBoxActions.TabStop = false;
            this.groupBoxActions.Text = "Действия с товарами";
            // 
            // btnAddProduct
            // 
            this.btnAddProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnAddProduct.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnAddProduct.Location = new System.Drawing.Point(10, 195);
            this.btnAddProduct.Name = "btnAddProduct";
            this.btnAddProduct.Size = new System.Drawing.Size(105, 35);
            this.btnAddProduct.TabIndex = 12;
            this.btnAddProduct.Text = "➕ ДОБАВИТЬ";
            this.btnAddProduct.UseVisualStyleBackColor = false;
            this.btnAddProduct.Click += new System.EventHandler(this.btnAddProduct_Click);
            // 
            // btnLoadFromFile
            // 
            this.btnLoadFromFile.Location = new System.Drawing.Point(115, 150);
            this.btnLoadFromFile.Name = "btnLoadFromFile";
            this.btnLoadFromFile.Size = new System.Drawing.Size(105, 35);
            this.btnLoadFromFile.TabIndex = 11;
            this.btnLoadFromFile.Text = "📂 Загрузить";
            this.btnLoadFromFile.UseVisualStyleBackColor = true;
            this.btnLoadFromFile.Click += new System.EventHandler(this.btnLoadFromFile_Click);
            // 
            // btnSortDesc
            // 
            this.btnSortDesc.Location = new System.Drawing.Point(115, 105);
            this.btnSortDesc.Name = "btnSortDesc";
            this.btnSortDesc.Size = new System.Drawing.Size(105, 35);
            this.btnSortDesc.TabIndex = 10;
            this.btnSortDesc.Text = "⬇️ По убыванию";
            this.btnSortDesc.UseVisualStyleBackColor = true;
            this.btnSortDesc.Click += new System.EventHandler(this.btnSortDesc_Click);
            // 
            // btnSortAsc
            // 
            this.btnSortAsc.Location = new System.Drawing.Point(10, 105);
            this.btnSortAsc.Name = "btnSortAsc";
            this.btnSortAsc.Size = new System.Drawing.Size(105, 35);
            this.btnSortAsc.TabIndex = 9;
            this.btnSortAsc.Text = "⬆️ По возрастанию";
            this.btnSortAsc.UseVisualStyleBackColor = true;
            this.btnSortAsc.Click += new System.EventHandler(this.btnSortAsc_Click);
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(10, 150);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(105, 35);
            this.btnOpenFile.TabIndex = 8;
            this.btnOpenFile.Text = "📂 Открыть";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // btnAddWithoutDiscount
            // 
            this.btnAddWithoutDiscount.Location = new System.Drawing.Point(115, 60);
            this.btnAddWithoutDiscount.Name = "btnAddWithoutDiscount";
            this.btnAddWithoutDiscount.Size = new System.Drawing.Size(105, 35);
            this.btnAddWithoutDiscount.TabIndex = 7;
            this.btnAddWithoutDiscount.Text = "🚫 Без скидки";
            this.btnAddWithoutDiscount.UseVisualStyleBackColor = true;
            this.btnAddWithoutDiscount.Click += new System.EventHandler(this.btnAddWithoutDiscount_Click);
            // 
            // btnAddWithDiscount
            // 
            this.btnAddWithDiscount.Location = new System.Drawing.Point(10, 60);
            this.btnAddWithDiscount.Name = "btnAddWithDiscount";
            this.btnAddWithDiscount.Size = new System.Drawing.Size(105, 35);
            this.btnAddWithDiscount.TabIndex = 6;
            this.btnAddWithDiscount.Text = "🏷️ Со скидкой";
            this.btnAddWithDiscount.UseVisualStyleBackColor = true;
            this.btnAddWithDiscount.Click += new System.EventHandler(this.btnAddWithDiscount_Click);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Location = new System.Drawing.Point(115, 15);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(105, 35);
            this.btnStatistics.TabIndex = 5;
            this.btnStatistics.Text = "📊 Статистика";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // btnSaveToCsv
            // 
            this.btnSaveToCsv.Location = new System.Drawing.Point(115, 195);
            this.btnSaveToCsv.Name = "btnSaveToCsv";
            this.btnSaveToCsv.Size = new System.Drawing.Size(105, 35);
            this.btnSaveToCsv.TabIndex = 4;
            this.btnSaveToCsv.Text = "💾 Сохранить";
            this.btnSaveToCsv.UseVisualStyleBackColor = true;
            this.btnSaveToCsv.Click += new System.EventHandler(this.btnSaveToCsv_Click);
            // 
            // btnFindCheapest
            // 
            this.btnFindCheapest.Location = new System.Drawing.Point(10, 15);
            this.btnFindCheapest.Name = "btnFindCheapest";
            this.btnFindCheapest.Size = new System.Drawing.Size(105, 35);
            this.btnFindCheapest.TabIndex = 3;
            this.btnFindCheapest.Text = "🏆 Дешевый";
            this.btnFindCheapest.UseVisualStyleBackColor = true;
            this.btnFindCheapest.Click += new System.EventHandler(this.btnFindCheapest_Click);
            // 
            // btnDeleteProduct
            // 
            this.btnDeleteProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnDeleteProduct.Location = new System.Drawing.Point(10, 195);
            this.btnDeleteProduct.Name = "btnDeleteProduct";
            this.btnDeleteProduct.Size = new System.Drawing.Size(105, 35);
            this.btnDeleteProduct.TabIndex = 2;
            this.btnDeleteProduct.Text = "❌ Удалить";
            this.btnDeleteProduct.UseVisualStyleBackColor = false;
            this.btnDeleteProduct.Visible = false; // Скрываем, так как теперь есть кнопка Добавить
            // 
            // btnEditProduct
            // 
            this.btnEditProduct.Location = new System.Drawing.Point(115, 195);
            this.btnEditProduct.Name = "btnEditProduct";
            this.btnEditProduct.Size = new System.Drawing.Size(105, 35);
            this.btnEditProduct.TabIndex = 1;
            this.btnEditProduct.Text = "✏️ Редактировать";
            this.btnEditProduct.UseVisualStyleBackColor = true;
            this.btnEditProduct.Visible = false; // Скрываем, так как переместили
            // 
            // groupBoxSearch
            // 
            this.groupBoxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxSearch.Controls.Add(this.btnRefresh);
            this.groupBoxSearch.Controls.Add(this.btnSearch);
            this.groupBoxSearch.Controls.Add(this.txtSearch);
            this.groupBoxSearch.Location = new System.Drawing.Point(12, 12);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(226, 150);
            this.groupBoxSearch.TabIndex = 0;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "Поиск товаров";
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(115, 90);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(105, 35);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "🔄 Обновить";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(10, 90);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(105, 35);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "🔍 Найти";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(10, 40);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(210, 20);
            this.txtSearch.TabIndex = 0;
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelBottom.Controls.Add(this.lblStatus);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 480);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(924, 42);
            this.panelBottom.TabIndex = 5;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(12, 15);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(48, 15);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Статус";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(924, 522);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelBottom);
            this.MinimumSize = new System.Drawing.Size(940, 560);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Интернет-магазин - Учет товаров";
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewProducts)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.groupBoxShopInfo.ResumeLayout(false);
            this.groupBoxShopInfo.PerformLayout();
            this.panelRight.ResumeLayout(false);
            this.groupBoxActions.ResumeLayout(false);
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.GroupBox groupBoxShopInfo;
        private System.Windows.Forms.Label lblTotalValue;
        private System.Windows.Forms.Label lblDiscountedCount;
        private System.Windows.Forms.Label lblProductCount;
        private System.Windows.Forms.Label lblShopName;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.GroupBox groupBoxActions;
        private System.Windows.Forms.Button btnAddWithoutDiscount;
        private System.Windows.Forms.Button btnAddWithDiscount;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Button btnSaveToCsv;
        private System.Windows.Forms.Button btnFindCheapest;
        private System.Windows.Forms.Button btnDeleteProduct;
        private System.Windows.Forms.Button btnEditProduct;
        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.DataGridView dataGridViewProducts;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.Button btnSortAsc;
        private System.Windows.Forms.Button btnSortDesc;
        private System.Windows.Forms.Button btnLoadFromFile;
        private System.Windows.Forms.Button btnAddProduct;
    }
}