namespace ExamResultApp
{
    using Microsoft.Azure.Cosmos.Table;

    public class ExamResultEntity : TableEntity
    {
        public ExamResultEntity() { }

        public ExamResultEntity(string studentId, string subjectName)
        {
            PartitionKey = studentId;
            RowKey = subjectName;
        }
        public double Score { get; set; }
    }
}
