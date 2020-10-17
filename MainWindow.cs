using System;
using Microsoft.Data.SqlClient;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Prueba_1
{
    class MainWindow : Window
    {        
        [UI] private Label lblId = null;
        [UI] private Label lblNombre = null;
        [UI] private Label lblconexion = null;
        [UI] private Label lblMensaje = null;
        [UI] private Entry txtId = null;
        [UI] private Entry txtNombre = null;
        [UI] private Button btnInsertar = null;
        [UI] private Button btnModificar = null;
        [UI] private Button btnBorrar = null;
        [UI] private Button btnBuscar = null;
        private SqlConnection conexion;

        public MainWindow() : this(new Builder("MainWindow.glade")) { }

        private MainWindow(Builder builder) : base(builder.GetObject("MainWindow").Handle)
        {
            builder.Autoconnect(this);
            DeleteEvent += Window_DeleteEvent;
            // ejemplo btn.Clicked += btn_Clicked;
            btnBuscar.Clicked += Buscar_btn_Clicked;
            btnInsertar.Clicked += Insertar_btn_Clicked;
            btnBorrar.Clicked += Borrar_btn_Clicked;
            btnModificar.Clicked += Modificar_btn_Clicked;

            try
            {
                conexion = new SqlConnection("Data Source=localhost;Initial Catalog=test;user =sa; password =Canciondulce01!");
                conexion.Open();               
                lblconexion.Text = "Conextado al BD test...";
            }
            catch (System.Exception)
            {
                lblconexion.Text = "Error al conectar...";
            }

        }
        private void Buscar_btn_Clicked(object sender, EventArgs a)
        {
            string qry = "SELECT * FROM pais WHERE estatus = 'a' and id = " + txtId.Text;
            SqlCommand consultar = new SqlCommand(qry, conexion);
            SqlDataReader rdr = consultar.ExecuteReader();

            if(rdr.HasRows)
                while (rdr.Read())
                {
                    txtId.Text = rdr["id"].ToString();
                    txtNombre.Text = rdr["nombre"].ToString();
                    lblMensaje.Text = "¡Busqueda Exitoso!";
                }
            else{
                lblMensaje.Text = "¡Busqueda sin Resultados!";
                txtId.Text = "";
                txtNombre.Text = "";
            }

            rdr.Close();
        }
        
        private void Insertar_btn_Clicked(object sender, EventArgs a)
        {
            string qry = "INSERT INTO pais (id, nombre, estatus) VALUES (" + txtId.Text + ",'" + txtNombre.Text + "', 'a')";
            SqlCommand insertar = new SqlCommand(qry, conexion);
            int rowsAffected = insertar.ExecuteNonQuery();
            lblMensaje.Text = rowsAffected.ToString() + " Afectado(s)...";
            txtId.Text = "";
            txtNombre.Text = "";
        }
        
        private void Borrar_btn_Clicked(object sender, EventArgs a)
        {
            string qry = "DELETE FROM pais WHERE id = " + txtId.Text;
            SqlCommand delete = new SqlCommand(qry, conexion);
            int rowsAffected = delete.ExecuteNonQuery();
            lblMensaje.Text = rowsAffected.ToString() + " Afectado(s)...";
            txtId.Text = "";
            txtNombre.Text = "";
        }
        
        private void Modificar_btn_Clicked(object sender, EventArgs a)
        {
            string qry = "UPDATE pais SET nombre = '" + txtNombre.Text + "' WHERE id = " + txtId.Text;
            SqlCommand insertar = new SqlCommand(qry, conexion);
            int rowsAffected = insertar.ExecuteNonQuery();
            lblMensaje.Text = "Se actualizaron " + rowsAffected.ToString() + " Registro(s)...";
            txtId.Text = "";
            txtNombre.Text = "";
        }
        
        private void Window_DeleteEvent(object sender, DeleteEventArgs a) => Application.Quit();
    }
}