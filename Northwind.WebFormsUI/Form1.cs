using Northwin.Business.Abstract;
using Northwin.Business.Concrete;
using Northwind.DataAccess.Abstract;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.DataAccess.Concrete.NHibernate;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = new ProductManager(new EfProductDal());
            _categoryService=new CategoryManager(new EfCategoryDal());
        }
        private IProductService _productService;
        private ICategoryService _categoryService;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadProducts();
        }

        private void LoadCategories()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryId";

            cbxCategoryAdd.DataSource = _categoryService.GetAll();
            cbxCategoryAdd.DisplayMember = "CategoryName";
            cbxCategoryAdd.ValueMember = "CategoryId";
        }

        private void LoadProducts()
        {
            dwgProduct.DataSource = _productService.GetAll();
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dwgProduct.DataSource = _productService.GetProductByCategoryId(Convert.ToInt32(cbxCategory.SelectedValue));
            }
            catch 
            {

                
            }
            
        }

        private void tbxProductName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxProductName.Text))
            {
                dwgProduct.DataSource = _productService.GetProductByProductName(tbxProductName.Text);
            }
            else
            {
                LoadProducts();
            }
            
        }

        private void btnProductAdd_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Add(new Product
                {
                    ProductName = tbxProductAdd.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryAdd.SelectedValue),
                    UnitPrice = Convert.ToDecimal(tbxUnitPriceAdd.Text),
                    QuantityPerUnit = tbxQuantityPerUnitAdd.Text,
                    UnitsInStock = Convert.ToInt16(tbxStockAdd.Text)
                });
                MessageBox.Show("Ürün Eklendi");
                LoadProducts();
                ScreenClear();
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            
        }

        private void ScreenClear()
        {
            tbxProductAdd.Clear();
            tbxUnitPriceAdd.Clear();
            tbxQuantityPerUnitAdd.Clear();
            tbxStockAdd.Clear();
            LoadCategories();
        }

        private void btnProductUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Update(new Product
                {
                    ProductId = Convert.ToInt32(dwgProduct.CurrentRow.Cells[0].Value),
                    ProductName = tbxProductAdd.Text,
                    CategoryId = Convert.ToInt32(cbxCategoryAdd.SelectedValue),
                    UnitPrice = Convert.ToDecimal(tbxUnitPriceAdd.Text),
                    QuantityPerUnit = tbxQuantityPerUnitAdd.Text,
                    UnitsInStock = Convert.ToInt16(tbxStockAdd.Text)
                });
                MessageBox.Show("Ürün Güncellendi");
                LoadProducts();
                ScreenClear();
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            
        }

        private void dwgProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var row = dwgProduct.CurrentRow;
            tbxProductAdd.Text = row.Cells[1].Value.ToString();
            cbxCategoryAdd.SelectedValue = row.Cells[2].Value;
            tbxUnitPriceAdd.Text = row.Cells[3].Value.ToString();
            tbxQuantityPerUnitAdd.Text = row.Cells[4].Value.ToString();
            tbxStockAdd.Text = row.Cells[5].Value.ToString();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if (dwgProduct.CurrentRow!=null)
            {
                try
                {
                    _productService.Delete(new Product
                    {
                        ProductId = Convert.ToInt32(dwgProduct.CurrentRow.Cells[0].Value)
                    });
                    MessageBox.Show("Ürün Silindi");
                    LoadProducts();
                    ScreenClear();
                }
                catch (Exception exception)
                {

                    MessageBox.Show(exception.Message);
                    ScreenClear();
                }
            }
            
        }
    }
}
