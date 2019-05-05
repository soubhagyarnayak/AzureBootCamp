namespace ExamResultApp
{
    using Microsoft.Azure.Cosmos.Table;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    class ExamResultsTable
    {
        const string TableName = "ExamResults";

        public static void Initialize()
        {
            var cloudTable = GetCloudTableClient();
            Upsert(cloudTable, new ExamResultEntity("CS001", "Compiler") { Score = 86.5 });
            Upsert(cloudTable, new ExamResultEntity("CS001", "Operating System") { Score = 82 });
            Upsert(cloudTable, new ExamResultEntity("CS001", "Computer Algorithms") { Score = 95 });
            Upsert(cloudTable, new ExamResultEntity("CS002", "Compiler") { Score = 88 });
            Upsert(cloudTable, new ExamResultEntity("CS002", "Operating System") { Score = 95 });
            Upsert(cloudTable, new ExamResultEntity("CS002", "Computer Algorithms") { Score = 90 });
            Upsert(cloudTable, new ExamResultEntity("CS003", "Compiler") { Score = 77 });
            Upsert(cloudTable, new ExamResultEntity("CS003", "Operating System") { Score = 82 });
            Upsert(cloudTable, new ExamResultEntity("CS003", "Computer Algorithms") { Score = 88.5 });
        }

        public static List<ExamResultEntity> GetResult(string studentId)
        {
            var result = new List<ExamResultEntity>();
            var cloudTable = GetCloudTableClient();
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, studentId);
            var tableQuery = new TableQuery<ExamResultEntity>().Where(filter).Select(new List<string> { "Score" });
            var tableResult = cloudTable.ExecuteQuery<ExamResultEntity>(tableQuery);
            return tableResult.ToList();
        }

        public static ExamResultEntity GetResult(string studentId, string courseId)
        {
            var cloudTable = GetCloudTableClient();
            var retrieveOperation = TableOperation.Retrieve<ExamResultEntity>(studentId, courseId);
            var operationResult = cloudTable.Execute(retrieveOperation);
            var examResult = operationResult.Result as ExamResultEntity;
            return examResult;
        }

        private static void Upsert(CloudTable table, ExamResultEntity entity)
        {
            TableOperation insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
            table.Execute(insertOrMergeOperation);
        }

        private static CloudTable GetCloudTableClient()
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            var cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var cloudTable = cloudTableClient.GetTableReference(TableName);
            cloudTable.CreateIfNotExists();
            return cloudTable;
        }
    }
}
