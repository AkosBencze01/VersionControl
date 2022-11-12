using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRF_3
{
    public partial class Form1 : Form
    {
        List<Country> countries = new List<Country>();

        List<Brand> brands = new List<Brand>();

        List<Ramen> ramens = new List<Ramen>();

        public Form1()
        {
            InitializeComponent();
            LoadData("ramen.csv");

            GetCountries();
            listBox1.DisplayMember = "Name";
            listBox1.ValueMember = "Name";

            textBox1.TextChanged += TextBox1_TextChanged;

            listBox1.SelectedIndexChanged += ListBox1_SelectedIndexChanged;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var country = (Country)((ListBox)sender).SelectedItem;
            if (country == null)
                return;

            var countryRamens = from r in ramens
                                where r.CountryFK == country.ID
                                select r;

            var groupedRamens = from r in countryRamens
                                group r.Rating by r.Brand.Name into g
                                select new
                                {
                                    BrandName = g.Key,
                                    AverageRating = Math.Round(g.Average(), 2)
                                };

            var orderedGroups = from g in groupedRamens
                                orderby g.AverageRating descending
                                select g;

            dataGridView1.DataSource = orderedGroups.ToList();
        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            GetCountries();
        }

        void LoadData(string filename)
        {
            using (StreamReader sr = new StreamReader(filename, Encoding.Default))
            {
                sr.ReadLine(); // Header

                while (!sr.EndOfStream)
                {
                    string[] adatok = sr.ReadLine().Split(';');

                    var countryName = adatok[2];
                    var brandName = adatok[0];
                    Country result=AddCountry(countryName);
                    Brand result2 = AddBrand(brandName);

                    var newRamen = new Ramen()
                    {
                        ID = ramens.Count + 1,
                        Brand = result2,
                        Name = adatok[1],
                        CountryFK = result.ID,
                        Country = result,
                        Rating = Convert.ToDouble(adatok[3])
                    };
                    ramens.Add(newRamen);
                }
            }
        }

        private Country AddCountry(string countryName)
        {
            var currentCountry = (from c in countries
                                  where c.Name.Equals(countryName)
                                  select c).FirstOrDefault();
            if (currentCountry == null)
            {
                currentCountry = new Country()
                {
                    ID = countries.Count + 1,
                    Name = countryName
                };
                countries.Add(currentCountry);
            }

            return currentCountry;
        }

        private Brand AddBrand(string brandName)
        {
            var currentBrand = (from c in brands
                                  where c.Name.Equals(brandName)
                                  select c).FirstOrDefault();
            if (currentBrand == null)
            {
                currentBrand = new Brand()
                {
                    ID = brands.Count + 1,
                    Name = brandName
                };
                brands.Add(currentBrand);
            }

            return currentBrand;
        }

        private void GetCountries()
        {
            var countriesList = from c in countries
                                where c.Name.ToLower().Contains(textBox1.Text.ToLower())
                                orderby c.Name
                                select c;
            listBox1.DataSource = countriesList.ToList();
        }
    }
}
