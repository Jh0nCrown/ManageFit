﻿using NET_MVC.Models;
using Oracle.ManagedDataAccess.Client;

namespace NET_MVC.Datos
{
    public class AdmCliente
    {
        public OracleConnection conexionBD = Conexion.GetConnection();

        public bool RegistrarCliente(ClienteModel cliente)
        {
            bool rpta = false; // Asegurarse de inicializar rpta
            try
            {
                if (Conexion.abrirConexion())
                {
                    // llamada a procedimiento almacenado en base de datos 
                    using (OracleCommand cmd = new OracleCommand("insertar_cliente", conexionBD))
                    {
                        // Especifica que es un procedimiento almacenado
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion; 
                        cmd.Parameters.Add("p_id_sede", OracleDbType.Int32).Value = cliente.IdSede;
                        cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = null; 
                        cmd.Parameters.Add("p_nombre_cliente", OracleDbType.Varchar2).Value = cliente.Nombre;
                        cmd.Parameters.Add("p_fecha_nacimiento", OracleDbType.Date).Value = cliente.FechaNacimiento;
                        cmd.Parameters.Add("p_telefono_cliente", OracleDbType.Varchar2).Value = cliente.Telefono;
                        cmd.Parameters.Add("p_genero_cliente", OracleDbType.Char).Value = cliente.Genero;

                        // Ejecutar el procedimiento almacenado
                        cmd.ExecuteNonQuery();
                    }
                        using (OracleCommand cmdMembresia = new OracleCommand("insertar_membresia", conexionBD))
                        {
                            cmdMembresia.CommandType = System.Data.CommandType.StoredProcedure;
                            cmdMembresia.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion;
                            cmdMembresia.Parameters.Add("p_tipo", OracleDbType.Varchar2).Value = cliente.refMembresia;
                            cmdMembresia.Parameters.Add("p_fecha_suscripcion", OracleDbType.Date).Value = DateTime.Now;

                            cmdMembresia.ExecuteNonQuery();
                        
                        }

                            rpta = true;
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error general: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion(); // Asegúrate de cerrar la conexión siempre
            }
            return rpta;
        }
    }
}