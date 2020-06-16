$(function () {
    $("#epdflist").click(async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/employeereport`);
            if (!response.ok) //check for response status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise so we await it
            data.msg === "Report Generated"
                ? window.open("/pdfs/employeelist.pdf")
                : $("#lblstatus").text("Problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        }
    });//buttonclick

    $("#cpdflist").click(async (e) => {
        try {
            $("#lblstatus").text("generating report on server - please wait...");
            let response = await fetch(`api/callreport`);
            if (!response.ok) //check for response status
                throw new Error(`Status - ${response.status}, Text - ${response.statusText}`);
            let data = await response.json(); // this returns a promise so we await it
            data.msg === "Report Generated"
                ? window.open("/pdfs/calllist.pdf")
                : $("#lblstatus").text("Problem generating report");
        } catch (error) {
            $("#lblstatus").text(error.message);
        }
    });//buttonclick

});//jquery