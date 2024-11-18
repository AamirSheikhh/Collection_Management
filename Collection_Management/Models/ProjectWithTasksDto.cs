﻿namespace Collection_Management.Models
{
    public class ProjectWithTasksDto
    {
        public int Id { get; set; }                 
        public string Title { get; set; }              
        public string Description { get; set; }       
        public string Status { get; set; }            
        public List<TaskDto> Tasks { get; set; }      
    }
    public class TaskDto
    {
        public int Id { get; set; }           
        public string Title { get; set; }         
        public string Description { get; set; }     
        public DateTime DueDate { get; set; }      
        public bool IsCompleted { get; set; }       
    }
}