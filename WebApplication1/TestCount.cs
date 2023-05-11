using Microsoft.AspNetCore.Mvc;

public class TestCount : ITestCount
{
    public int Count { get; set; }

    public TestCount(ILogger<TestCount> logger)
	{
		logger.LogInformation("In TesCount");
	}
}

public class ITestCount
{
    int Count { get; set; }
}