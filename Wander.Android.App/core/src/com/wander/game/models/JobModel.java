package com.wander.game.models;

/**
 * Created by valentin on 15/12/2015.
 */
public class JobModel {
    @com.google.gson.annotations.SerializedName("Jobid")
    public int JobId;
    @com.google.gson.annotations.SerializedName("JobDescription")
    public String JobDescription;
    @com.google.gson.annotations.SerializedName("Salary")
    public int Salary;
    @com.google.gson.annotations.SerializedName("Threshold")
    public int Threshold;
    @com.google.gson.annotations.SerializedName("NecessaryPoints")
    public int NecessaryPoints;
    @com.google.gson.annotations.SerializedName("EarningPoints")
    public int EarningPoints;

    @Override
    public String toString()
    {
        if(this.JobDescription == null) return "null";
        return this.JobDescription + " Necessary points : " + this.NecessaryPoints;
    }
}
