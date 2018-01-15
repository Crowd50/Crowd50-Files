namespace Crowd50_DataAccess
{
    using System;
    using System.Linq;
    using System.Data;
    using AutoMapping;
    using CommandExecution;
    using System.Data.SqlClient;
    using System.Collections.Generic;
    using Crowd50_DataAccess.Models;

    public class TicketDataAccess : CommandExecutor 

    {
        public TicketDataAccess(string iConnectionString, string iLogFile)
            : base(iConnectionString, iLogFile)
            {
            }

        public List<Ticket> ViewAllTickets()
        {
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_ALL_TICKET");
            return AutoMap<Ticket>.FromDataTable(lQueryResult).ToList();
        }

        public Ticket ViewTicketById(Int64 TicketId)
        {
            Ticket response = null;
            DataTable lQueryResult = ExecuteProcedureQuery("VIEW_TICKET_BY_ID",
                new SqlParameter("@TicketId", TicketId),
                iCommandTimeout: 30);
            if (lQueryResult.Rows.Count > 0)
            {
                response = AutoMap<Ticket>.FromDataRow(lQueryResult.Rows[0]);
            }
            return response;
        }

        public bool CreateNewTicket(Ticket ticket)
        {
            return ExecuteProcedureNonQuery("CREATE_NEW_TICKET",
                new List<SqlParameter>()
                {
                    new SqlParameter("@CampaignId", ticket.CampaignId),
                    new SqlParameter("@UserId", ticket.UserId),
                    new SqlParameter("@GenerationDate", ticket.GenerationDate),
                    new SqlParameter("@ValidationCode", ticket.ValidationCode),
                },
                iCommandTimeout: 60);
        }

        public bool UpdateTicket(Ticket ticket)
        {
            return ExecuteProcedureNonQuery("UPDATE_TICKET",
                new List<SqlParameter>()
                {
                    new SqlParameter("@TicketId", ticket.TicketId),
                    new SqlParameter("@CampaignId", ticket.CampaignId),
                    new SqlParameter("@UserId", ticket.UserId),
                    new SqlParameter("@GenerationDate", ticket.GenerationDate),
                    new SqlParameter("@ValidationCode", ticket.ValidationCode),
                },
                iCommandTimeout: 60);
        }

        public bool DeleteTicket(Int64 TicketId)
        {
            return ExecuteProcedureNonQuery("DELETE_TICKET",
                new SqlParameter("@TicketId", TicketId), 30);
        }

    }
}
