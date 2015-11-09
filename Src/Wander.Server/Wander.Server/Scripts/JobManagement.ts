hub.on("onGetJobs", function (jobs) {
    $("#jobsModalBody").text("");
    for (var i = 0; i < jobs.length; i++) {
        $("#jobsModalBody").append("<tr>"+
            "<td> " + jobs[i].JobDescription + " </td>" +
            "<td> " + jobs[i].Salary + " </td>" +
            "<td> " + jobs[i].NecessaryPoints + " </td>" +
            "<td> " + jobs[i].EarningPoints + " </td>" +
            "<td> " + jobs[i].Threshold + " </td>" +
            "<td> <button class='btn btn-success' onclick='applyToJob(" + jobs[i].JobId + ")' > Apply </button></td></tr>");
    }
});

function getAllJobs() {
    hub.invoke("GetAllJobs");
}

function applyToJob(id) {
    hub.invoke("ApplyJob", id);
}