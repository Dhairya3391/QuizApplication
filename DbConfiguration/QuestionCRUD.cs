﻿using System.Data;
using System.Data.SqlClient;
using QuizApplication.Models;

namespace QuizApplication.DbConfiguration;

public class QuestionCRUD
{
    private readonly IConfiguration configuration;

    public QuestionCRUD(IConfiguration _configuration)
    {
        configuration = _configuration;
    }

    public bool AddQuestion(QuestionModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = new SqlCommand("PR_Question_Insert", connection);
        command.CommandType = CommandType.StoredProcedure;

        command.Parameters.AddWithValue("@QuestionText", model.QuestionText);
        command.Parameters.AddWithValue("@OptionA", model.OptionA);
        command.Parameters.AddWithValue("@OptionB", model.OptionB);
        command.Parameters.AddWithValue("@OptionC", model.OptionC);
        command.Parameters.AddWithValue("@OptionD", model.OptionD);
        command.Parameters.AddWithValue("@CorrectOption", model.CorrectOption);
        command.Parameters.AddWithValue("@QuestionMarks", model.QuestionMarks);
        command.Parameters.AddWithValue("@IsActive", model.IsActive);
        command.Parameters.AddWithValue("@Created", DateTime.Now);
        command.Parameters.AddWithValue("@Modified", DateTime.Now);
        command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
        command.Parameters.AddWithValue("@QuestionLevelID", model.QuestionLevelID);

        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public DataTable EditQuestion(int questionId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = new SqlCommand("PR_Question_SelectByPK", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
        command.Parameters.AddWithValue("@QuestionID", questionId);

        var reader = command.ExecuteReader();

        var table = new DataTable();
        table.Load(reader);
        return table;
    }

    //public bool UpdateQuestion(QuizModel model)
    //{
    //    string connectionString = configuration.GetConnectionString("ConnectionString");

    //    SqlConnection connection = new SqlConnection(connectionString);
    //    connection.Open();
    //    SqlCommand command = new SqlCommand("PR_Quiz_Insert", connection);
    //    command.CommandType = CommandType.StoredProcedure;

    //    command.Parameters.AddWithValue("@QuizName", model.QuizName);
    //    command.Parameters.AddWithValue("@TotalQuestions", model.TotalQuestions);
    //    command.Parameters.AddWithValue("@QuizDate", model.QuizDate);
    //    command.Parameters.AddWithValue("@UserID", 1);
    //    int result = command.ExecuteNonQuery();
    //    return result > 0;

    //}

    public bool UpdateQuestion(QuestionModel model)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Question_UpdateByPk", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuestionID", model.QuestionID);
            command.Parameters.AddWithValue("@QuestionText", model.QuestionText);
            command.Parameters.AddWithValue("@OptionA", model.OptionA);
            command.Parameters.AddWithValue("@OptionB", model.OptionB);
            command.Parameters.AddWithValue("@OptionC", model.OptionC);
            command.Parameters.AddWithValue("@OptionD", model.OptionD);
            command.Parameters.AddWithValue("@CorrectOption", model.CorrectOption);
            command.Parameters.AddWithValue("@QuestionMarks", model.QuestionMarks);
            command.Parameters.AddWithValue("@IsActive", model.IsActive);

            command.Parameters.AddWithValue("@QuestionLevelID", model.QuestionLevelID);
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());
            command.Parameters.AddWithValue("@Created", model.Created);
            command.Parameters.AddWithValue("@Modified", model.Modified);


            var result = command.ExecuteNonQuery();
            return result > 0;
        }
    }


    public bool DeleteQuestion(int questionId)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");

        var connection = new SqlConnection(connectionString);
        connection.Open();
        var command = new SqlCommand("PR_Question_DeleteByPk", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());

        command.Parameters.AddWithValue("@QuestionID", questionId);
        var result = command.ExecuteNonQuery();
        return result > 0;
    }

    public DataTable SearchQuestions(string questionText, string correctOptions, int? questionMarks)
    {
        var connectionString = configuration.GetConnectionString("ConnectionString");
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            var command = new SqlCommand("PR_Question_Search", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@QuestionText", (object)questionText ?? DBNull.Value);
            command.Parameters.AddWithValue("@CorrectOptions", (object)correctOptions ?? DBNull.Value);
            command.Parameters.AddWithValue("@QuestionMarks", (object)questionMarks ?? DBNull.Value);
            command.Parameters.AddWithValue("@UserID", SessionVariables.UserID());

            var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Load(reader);
            return table;
        }
    }
}