﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore.Query.Internal;
using NET_MVC.Models;
using NuGet.Protocol.Plugins;
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

                        // Castear
                        int id = int.Parse(cliente.Identificacion);
                        int idSede = int.Parse(cliente.IdSede);

                        // Agregar parámetros para el procedimiento almacenado
                        cmd.Parameters.Add("p_id_cliente", OracleDbType.Int32).Value = cliente.Identificacion;
                        if(cliente.IdEntrenador == 0 || cliente.IdEntrenador == null){
                            cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = null;
                        }
                        else{
                            cmd.Parameters.Add("p_id_entrenador", OracleDbType.Int32).Value = cliente.IdEntrenador;
                        }
                        cmd.Parameters.Add("p_fecha_nacimiento", OracleDbType.Date).Value = cliente.FechaNacimiento;

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
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
            return rpta;
        }

        public string cadenaListarClientes(string filter, string IdSede)
        {
            string sql = "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                     "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                     "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                     "WHERE p.id_sede = " + IdSede +
                     " GROUP BY c.id_cliente";

            switch (filter)
            {
                case "all":
                    break;
                case "premium":
                    sql = cadenasql2(1, IdSede); // Clientes premium
                    break;
                case "general":
                    sql = cadenasql2(2, IdSede); // Clientes generales
                    break;
                case "masculino":
                    sql = cadenasql3(1, IdSede); // Género Masculino
                    break;
                case "femenino":
                    sql = cadenasql3(2, IdSede); // Género Femenino
                    break;
                case "no-especificado":
                    sql = cadenasql3(3, IdSede); // Género No especificado
                    break;
                default:
                    break;
            }
            return sql;
        }
        private string cadenasql2(int opcion, String IdSede)
        {
            string nomMembresia = opcion == 1 ? "Premium" :
                                  "General";

            return "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND m.tipo = '" + nomMembresia + "' " +
                                "GROUP BY c.id_cliente";
        }
        private string cadenasql3(int opcion, String IdSede)
        {
            string genero = opcion == 1 ? "M" :
                            opcion == 2 ? "F" :
                            "NE";

            return "SELECT c.id_cliente, MAX(p.nombre_persona) AS nombre, MAX(p.telefono_persona) AS telefono,MAX(m.tipo) AS membresia, TO_CHAR(MAX(m.fecha_suscripcion), 'DD-MM-YYYY') AS fecha " +
                                "FROM cliente c INNER JOIN persona p ON c.id_cliente = p.id_persona " +
                                "INNER JOIN membresia m ON c.id_cliente = m.id_cliente " +
                                "WHERE p.id_sede = " + IdSede + " AND p.genero_persona = '" + genero + "' " +
                                "GROUP BY c.id_cliente";
        }
        public List<ClienteModel> ListarClientes(string sql)
        {
            var clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            ClienteModel objcliente = new ClienteModel
                            {
                                Identificacion = reader["id_cliente"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                refMembresia = reader["membresia"].ToString(),
                                fechaMembresia = reader["fecha"].ToString(),
                            };
                            clientes.Add(objcliente);
                        }
                    }
                }
                return clientes;
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
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
        }
        public List<ClienteModel> ListarClientesAsignados(string idEntrenador)
        {
            List<ClienteModel> clientes = new List<ClienteModel>();
            try
            {
                if (Conexion.abrirConexion())
                {
                    string sql = "SELECT CLIENTE.id_cliente AS id_cliente, PERSONA.nombre_persona AS nombre, PERSONA.telefono_persona AS telefono, MEMBRESIA.tipo AS membresia,calcular_dias_restantes(CLIENTE.id_cliente) AS dias_restantes " +
                                 "FROM cliente" +
                                 "    INNER JOIN persona ON cliente.id_cliente = persona.id_persona" +
                                 "    INNER JOIN membresia ON cliente.id_cliente = membresia.id_cliente" +
                                 "    WHERE CLIENTE.id_entrenador = " + idEntrenador;
                    using (OracleCommand query = new OracleCommand(sql, conexionBD))
                    {
                        OracleDataReader reader = query.ExecuteReader();
                        while (reader.Read())
                        {
                            ClienteModel objcliente = new ClienteModel
                            {
                                Identificacion = reader["id_cliente"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                Telefono = reader["telefono"].ToString(),
                                refMembresia = reader["membresia"].ToString(),
                                DiasRestantes = Convert.ToInt32(reader["dias_restantes"])
                            };
                            clientes.Add(objcliente);
                        }
                    }
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
                Conexion.cerrarConexion(); //Cerrar la conexión
            }
            return clientes;
        }
        public bool eliminarCliente(string identificacion)
        {
            bool existe = false;
            if (!int.TryParse(identificacion, out int idPersona))
            {
                throw new Exception("La identificación debe ser un número entero válido.");
            }
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM MEMBRESIA WHERE id_cliente = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idPersona)); //eliminacion de membresia
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                    using (OracleCommand cmd = new OracleCommand("DELETE FROM CLIENTE WHERE id_cliente = :id", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter("id", idPersona)); //eliminacion de registro de cliente
                        int count = Convert.ToInt32(cmd.ExecuteScalar());
                        existe = count > 0;
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Error en Oracle: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return existe;
        }

        public bool ClienteExiste(string identificacion)
        {
            bool existe = false;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT COUNT(*) FROM Cliente WHERE id_cliente = :p_id_cliente", conexionBD))
                    {
                        cmd.Parameters.Add(":p_id_cliente", OracleDbType.Varchar2).Value = identificacion;

                        int count = Convert.ToInt32(cmd.ExecuteScalar());

                        // Si el contador es mayor a 0, significa que el cliente existe
                        existe = count > 0;
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return existe;
        }

        public ClienteModel ObtenerClientePorIdentificacion(string identificacion, string idSede)
        {
            ClienteModel clienteEncontrado = null;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(
                      "SELECT c.id_cliente, p.nombre_persona AS nombre, p.telefono_persona AS telefono, p.genero_persona AS genero, m.tipo, " +
                      "calcular_dias_restantes(c.id_cliente) AS dias_restantes " +
                      "FROM Cliente c " +
                      "JOIN Persona p ON c.id_cliente = p.id_persona " +
                      "JOIN Membresia m ON c.id_cliente = m.id_cliente " +
                      "WHERE c.id_cliente = :p_id_cliente AND id_sede = :p_id_sede", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add(":p_id_cliente", OracleDbType.Int32).Value = int.Parse(identificacion);
                        cmd.Parameters.Add(":p_id_sede", OracleDbType.Int32).Value = int.Parse(idSede);

                        using (OracleDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clienteEncontrado = new ClienteModel
                                {
                                    Identificacion = reader["id_cliente"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Telefono = reader["telefono"].ToString(),
                                    Genero = reader["genero"].ToString(),
                                    refMembresia = reader["tipo"].ToString(),
                                    DiasRestantes = Convert.ToInt32(reader["dias_restantes"])
                                };
                            }
                        }
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return clienteEncontrado;
        }

        // GRAFICAS DEL INICIO: 
        public int ObtenerTotalClientesPorSede(int idSede)
        {
            int totalClientes = 0;
            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand("SELECT COUNT(*) FROM Cliente c JOIN Persona p ON c.id_Cliente = p.id_Persona WHERE p.id_Sede = :idSede", conexionBD))
                    {
                        cmd.Parameters.Add(new OracleParameter(":idSede", idSede));
                        totalClientes = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Error en Oracle: " + ex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }
            return totalClientes;
        }

        public int ObtenerTotalClientesPorTipo(int idSede, string tipoMembresia)
        {
            int total = 0;

            try
            {
                if (Conexion.abrirConexion())
                {
                    using (OracleCommand cmd = new OracleCommand(@"
                SELECT COUNT(*)
                FROM Cliente c
                JOIN Persona p ON c.id_Cliente = p.id_Persona
                JOIN Membresia m ON c.id_Cliente = m.id_Cliente
                WHERE p.id_Sede = :idSede AND m.tipo = :tipoMembresia", conexionBD))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.Add("idSede", OracleDbType.Int32).Value = idSede;
                        cmd.Parameters.Add("tipoMembresia", OracleDbType.Varchar2).Value = tipoMembresia;

                        total = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
            }
            catch (OracleException oex)
            {
                throw new Exception("Error en Oracle: " + oex.Message);
            }
            finally
            {
                Conexion.cerrarConexion();
            }

            return total;
        }

    }
}
