
// script to verify a test run has passed
// it is possible for nunit-console.exe to return 0 when there are teardown failures:
// 
// run using WSH (CScript.exe)

var args = WScript.Arguments;

if (args.length != 1)
    throw new Error("Usage: VerifyPass.js <Assembly.results.xml>");

var resultsFilename = args(0);

var summaryDoc = new ActiveXObject("Microsoft.XMLDOM");
summaryDoc.load(resultsFilename);

var failedSuites = summaryDoc.selectNodes('//test-suite[@result="Failure"]');

if (failedSuites.length > 0)
    throw new Error("detected failures in " + resultsFilename);

WScript.Echo("No failed suites detect in " + resultsFilename);