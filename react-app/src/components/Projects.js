import React, { useState, useEffect } from "react";
import { connect } from "react-redux";
import * as actions from "../actions/ProjectsAct";

const Projects = (props) => {

    useEffect(()=>{
        props.fetchAllProjects()
    },[])

    return (<div>from Projects</div>);
}

const mapStateToProps = state => ({
        projectsList:state.ProjectsRed.list
})

const mapActionToProps ={
    fetchAllProjects: actions.fetchall
}

// connect() is a function witch returns another function with "Projects" as a parameter
export default connect(mapStateToProps,mapActionToProps)(Projects);