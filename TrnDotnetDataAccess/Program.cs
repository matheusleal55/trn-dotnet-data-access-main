﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using TrnDotnetDataAccess.Entidades;

namespace TrnDotnetDataAccess
{
    class Program
    {
        private static SqlConnection sqlConnection;
        static void Main(string[] args)
        {
            IniciarConexao();

            GravarNovoCliente();
            GravarNovoProduto();
            ListaProdutos();

            Console.ReadKey();
        }
        private static void IniciarConexao()
        {
            var connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=dbLoja;Integrated Security=True;Connect Timeout=30;";

            sqlConnection = new SqlConnection();
            sqlConnection.ConnectionString = connectionString;
        }
        private static void GravarNovoCliente()
        {
            IniciarConexao();
            sqlConnection.Open();

            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "insert into Cliente values(@id,@nome,@email,@senha)";

            var cliente = new Cliente("Maria da Silva", "marias274@gmail.com", "123456");

            sqlCommand.Parameters.Add(new SqlParameter("@id",cliente.Id));
            sqlCommand.Parameters.Add(new SqlParameter("@nome", cliente.Nome));
            sqlCommand.Parameters.Add(new SqlParameter("@email", cliente.Email));
            sqlCommand.Parameters.Add(new SqlParameter("@senha", cliente.Senha));

            var qtdRows=sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Cliente cadastrado com sucesso");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();
        }
        private static void ExcluirCliente()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "delete from Cliente where id=@id";

            var clienteId = "B1A7280E-D71D-4169-97F3-083ED04C0ACB";
            sqlCommand.Parameters.Add(new SqlParameter("@id", clienteId));

            var qtdRows = sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Cliente excluído com sucesso");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();


        }
        private static void ListarClientes()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "select Id,Nome,Email from Cliente";

            var sqlDataReader = sqlCommand.ExecuteReader();

            List<Cliente> listaClientes = new List<Cliente>();

            while (sqlDataReader.Read())
            {
                Guid id = Guid.Parse(sqlDataReader[0].ToString());
                var cliente = new Cliente(id);
                cliente.Atualizar(sqlDataReader[1].ToString(), sqlDataReader[2].ToString());
                listaClientes.Add(cliente);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            sqlConnection.Dispose();

            foreach (var item in listaClientes)
            {
                Console.WriteLine($"Nome: {item.Nome}  - Email: {item.Email}");
            }


        }

        private static void GravarNovoProduto()
        {

            IniciarConexao();
            sqlConnection.Open();

            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "Insert into Produtos values(@id,@nome,@precoUnitario,@quantidadeEstoque)";

            var produto = new Produto("Xbox", 1500, 200);

            sqlCommand.Parameters.Add(new SqlParameter("@id", produto.Id));
            sqlCommand.Parameters.Add(new SqlParameter("@nome", produto.Nome));
            sqlCommand.Parameters.Add(new SqlParameter("@precoUnitario", produto.PrecoUnitario));
            sqlCommand.Parameters.Add(new SqlParameter("@quantidadeEstoque", produto.QuantidadeEstoque));

            var qtdRows = sqlCommand.ExecuteNonQuery();

            if (qtdRows > 0)
            {
                Console.WriteLine("Produto foi cadastrado");
            }

            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private static void ListaProdutos()
        {
            IniciarConexao();
            sqlConnection.Open();
            var sqlCommand = new SqlCommand();
            sqlCommand.Connection = sqlConnection;
            sqlCommand.CommandText = "Select Id, Nome, PrecoUnitario, QuantidadeEstoque from Produto";

            var sqlDataReader = sqlCommand.ExecuteReader();

            List<Produto> listaProdutos = new List<Produto>();

            while (sqlDataReader.Read())
            {
                Guid id = Guid.Parse(sqlDataReader[0].ToString());
                var produto = new Produto(sqlDataReader[1].ToString(), decimal.Parse(sqlDataReader[2].ToString()), int.Parse(sqlDataReader[3].ToString()));
                listaProdutos.Add(produto);
            }

            sqlDataReader.Close();
            sqlConnection.Close();
            sqlConnection.Dispose();

            foreach (var item in listaProdutos)
            {
                Console.WriteLine($"Nome: {item.Nome} - Preço do produto: {item.PrecoUnitario} - Quantidade em estoque: {item.QuantidadeEstoque}");
            }
        }



    }
}
