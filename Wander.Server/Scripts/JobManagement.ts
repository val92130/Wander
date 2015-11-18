hub.on("onGetJobs", function (jobs) {
    if (currentUser == "unedfined" || currentUser == null) return;
    $("#jobsModalBody").text("");
    for (var i = 0; i < jobs.length; i++) {
       
        $("#jobsModalBody").append("<tr>" +
            (currentUser.Job.JobId == jobs[i].JobId ? "<td><b>" + jobs[i].JobDescription + "</b> </td>" : "<td>" + jobs[i].JobDescription + " </td>")+
            "<td> " + jobs[i].Salary + " </td>" +
            "<td> " + jobs[i].NecessaryPoints + " </td>" +
            "<td> " + jobs[i].EarningPoints + " </td>" +
            "<td> " + jobs[i].Threshold + " </td>" +
            "<td> <button class='" + ((currentUser.Job.JobId == jobs[i].JobId || currentUser.Points >= jobs[i].NecessaryPoints) ? "btn btn-success" : "btn btn-danger") + "' " + ((currentUser.Points < jobs[i].NecessaryPoints || currentUser.Job.JobId == jobs[i].JobId) ? " disabled" : " ") + " onclick='applyToJob(" + jobs[i].JobId + ")' > Apply </button></td></tr>");
    }
});

function getAllJobs() {
    hub.invoke("GetAllJobs");
    $("#jobsModal").modal();
}

function applyToJob(id) {
    hub.invoke("ApplyJob", id);
}