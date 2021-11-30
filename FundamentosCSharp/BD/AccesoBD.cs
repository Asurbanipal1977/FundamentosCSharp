using FundamentosCSharp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace FundamentosCSharp.BD
{
    public class AccesoBD
    {
        private const string cadenaConexion = @"Data Source=GIGABYTE-SABRE\SQLEXPRESS;Initial Catalog=pruebas;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private SqlConnection Conexion { 
            get
            {
                return new SqlConnection(cadenaConexion);
            }
        }      


        public List<Cerveza> ListarCervezas ()
        {
            List<Cerveza> lista = new List<Cerveza>();
            string consulta = "select Id, Nombre, Marca,Alcohol, Cantidad from Cervezas";

            using (var conexion = Conexion)
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(consulta, conexion);
                SqlDataReader reader = cmd.ExecuteReader();
                

                while (reader.Read())
                {
                    Cerveza cerveza = new Cerveza(int.Parse(reader["Cantidad"].ToString()))
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Nombre = reader["Nombre"].ToString(),
                        Marca = reader["Marca"].ToString(),
                        Alcohol = int.Parse(reader["Alcohol"].ToString())
                    };

                    lista.Add(cerveza);
                }

                reader.Close();
                conexion.Close();
            }

            return lista;
        }

        public void Add (Cerveza cerveza)
        {
            string query = "insert into cervezas (Nombre, Marca, Alcohol, Cantidad) values (@Nombre, @Marca, @Alcohol, @Cantidad)";

            try
            {
                using (var conexion = Conexion)
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Nombre", cerveza.Nombre);
                    cmd.Parameters.AddWithValue("@Marca", cerveza.Marca);
                    cmd.Parameters.AddWithValue("@Alcohol", cerveza.Alcohol);
                    cmd.Parameters.AddWithValue("@Cantidad", cerveza.Cantidad);

                    int insercion = cmd.ExecuteNonQuery();
                    if (insercion > 0)
                    {
                        Console.WriteLine("Inserción realizada correctamente");
                    }

                    conexion.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Es un error al insertar");
            }
        }

        public void Edit(Cerveza cerveza)
        {
            string query = "update cervezas set Nombre=@Nombre, Marca=@Marca, Alcohol=@Alcohol, Cantidad=@Cantidad where Id=@Id";

            try
            {
                using (var conexion = Conexion)
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Id", cerveza.Id);
                    cmd.Parameters.AddWithValue("@Nombre", cerveza.Nombre);
                    cmd.Parameters.AddWithValue("@Marca", cerveza.Marca);
                    cmd.Parameters.AddWithValue("@Alcohol", cerveza.Alcohol);
                    cmd.Parameters.AddWithValue("@Cantidad", cerveza.Cantidad);

                    int insercion = cmd.ExecuteNonQuery();
                    if (insercion > 0)
                    {
                        Console.WriteLine("Modificación realizada correctamente");
                    }

                    conexion.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Es un error al Modificar");
            }
        }

        public void Delete(Cerveza cerveza)
        {
            string query = "delete from cervezas where Id=@Id";

            try
            {
                using (var conexion = Conexion)
                {
                    conexion.Open();
                    SqlCommand cmd = new SqlCommand(query, conexion);
                    cmd.Parameters.AddWithValue("@Id", cerveza.Id);
                    
                    int insercion = cmd.ExecuteNonQuery();
                    if (insercion > 0)
                    {
                        Console.WriteLine("Eliminación realizada correctamente");
                    }

                    conexion.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Es un error al Borrar");
            }
        }
    }
}
