function getAllJobs() {
    hub.invoke("GetAllJobs").done(function (jobs) {
        if (jobs != null && jobs != undefined) {
            $("#jobsModal").modal();
            if (currentUser == "unedfined" || currentUser == null)
                return;
            $("#jobsModalBody").text("");
            for (var i = 0; i < jobs.length; i++) {
                $("#jobsModalBody").append("<tr>" +
                    (currentUser.Job.JobId == jobs[i].JobId ? "<td><b>" + jobs[i].JobDescription + "</b> </td>" : "<td>" + jobs[i].JobDescription + " </td>") +
                    "<td> " + jobs[i].Salary + " </td>" +
                    "<td> " + jobs[i].NecessaryPoints + " </td>" +
                    "<td> " + jobs[i].EarningPoints + " </td>" +
                    "<td> " + jobs[i].Threshold + " </td>" +
                    "<td> <button class='" + ((currentUser.Job.JobId == jobs[i].JobId || currentUser.Points >= jobs[i].NecessaryPoints) ? "btn btn-success" : "btn btn-danger") + "' " + ((currentUser.Points < jobs[i].NecessaryPoints || currentUser.Job.JobId == jobs[i].JobId) ? " disabled" : " ") + " onclick='applyToJob(" + jobs[i].JobId + ")' > Apply </button></td></tr>");
            }
        }
    });
}
function applyToJob(id) {
    hub.invoke("ApplyJob", id).done(function () {
        $("#jobsModal").modal("hide");
    });
}
//# sourceMappingURL=JobManagement.js.map