using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ApplicationA
{
    public partial class ApplicationA : Form
    {
        CheckEntities entities;
        Integrator integrator;

        public ApplicationA()
        {
            InitializeComponent();
            entities = new CheckEntities();
            SerializationHandler serializationHandler = new SerializationHandler();
            String executablePath;

            try {
                executablePath = serializationHandler.deserialize();
            } catch (Exception e) {
                MessageBox.Show(
                    "Aplicación B no encontrada, por favor seleccione la ruta del ejecutable de la AplicaciónB",
                    "Seleccione",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                do
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.ShowDialog();
                    executablePath = ofd.FileName;
                } while (executablePath == null);

                try
                {
                    serializationHandler.serialize(executablePath);
                }
                catch (Exception e1)
                {
                    MessageBox.Show("Ha ocurrido un error: " + e1.Message);
                }
            }

            integrator = new Integrator(executablePath);

            var clients = (from client in entities.clients
                           select client).ToList();

            cbClient.SelectedIndexChanged -= new EventHandler(cbClient_SelectedIndexChanged);
            cbClient.DataSource = clients;
            cbClient.DisplayMember = "name";
            cbClient.SelectedIndex = -1;
            cbClient.SelectedIndexChanged += new EventHandler(cbClient_SelectedIndexChanged);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int number = 0;
            decimal amount = 0;
            DateTime date = dtPaymentDate.Value;
            String amountInLetter = txtAmountInLetters.Text.Trim();
            String paymentConcept = txtPaymentConcept.Text.Trim();
            clients client = (clients)cbClient.SelectedValue;
            invoices invoice = (invoices)cbInvoice.SelectedValue;

            Int32.TryParse(txtCheckNumber.Text, out number);
            Decimal.TryParse(txtAmount.Text, out amount);

            if (number == 0)
            {
                MessageBox.Show(
                    "Debes colocar el número de cheque",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (paymentConcept.Length == 0)
            {
                MessageBox.Show(
                    "Debes colocar un concepto de pago",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (amount == 0)
            {
                MessageBox.Show(
                    "Debes colocar un monto",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (client == null)
            {
                MessageBox.Show(
                    "Debes seleccionar un cliente",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (dtPaymentDate == null)
            {
                MessageBox.Show(
                    "Debes seleccionar una fecha de pago",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (amountInLetter.Length == 0)
            {
                MessageBox.Show(
                    "Debes colocar el monto en letras",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            if (invoice == null)
            {
                MessageBox.Show(
                    "Debes seleccionar una factura",
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation
                );
                return;
            }

            entities.checks.Add(new checks() {
                number = number,
                concept = paymentConcept,
                client_id = client.id,
                payment_date = date,
                amount = amount,
                amount_in_letters = amountInLetter,
                invoice_id = invoice.id
            });

            try
            {
                entities.SaveChanges();
                MessageBox.Show(
                    "¡El pago ha sido registrado exitosamente!",
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
                resetForm();
            } catch (Exception e1) {
                MessageBox.Show(
                    "Ha ocurrido un error: \n" + e1.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void resetForm() {
            txtCheckNumber.Text = "";
            txtPaymentConcept.Text = "";
            txtAmount.Text = "";
            cbClient.SelectedIndex = -1;
            dtPaymentDate.Value = new DateTime();
            txtAmountInLetters.Text = "";
            cbInvoice.SelectedIndex = -1;
            pbInvoice.Image = null;
        }

        private void cbClient_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbClient.SelectedValue != null)
            {
                progressBar.Value = 0;
                clients client = (clients) cbClient.SelectedValue;
                var invoices = (from invoice in entities.invoices
                                where invoice.client_id == client.id
                                select invoice).ToList();

                progressBar.Value = 30;
                cbInvoice.DataSource = invoices;
                cbInvoice.DisplayMember = "description";
                cbInvoice.SelectedIndex = -1;
                cbInvoice.Enabled = true;
                progressBar.Value = 50;
                try
                {
                    String imagePath = integrator.getImagePath(client.cedula);
                    progressBar.Value = 80;
                    String fullPath = Path.Combine(
                        Path.GetDirectoryName(integrator.getExecutablePath()),
                        imagePath
                    );

                    if (File.Exists(fullPath))
                    {
                        progressBar.Value = 90;
                        pbInvoice.Image = Image.FromFile(fullPath);
                    } else
                    {
                        MessageBox.Show("Imágen no encontrada", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        pbInvoice.Image = null;
                    }
                } catch (Exception e1) {
                    MessageBox.Show(e1.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    pbInvoice.Image = null;
                }
                progressBar.Value = 100;
            } else
            {
                cbInvoice.Enabled = false;
                cbInvoice.DataSource = new List<object>();
                pbInvoice.Image = null;
            }
        }
    }
}
