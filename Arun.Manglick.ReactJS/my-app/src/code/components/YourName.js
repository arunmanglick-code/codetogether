import classes from '../../css/NewPost.module.css';
import { useState } from "react";

function YourName(props) {

    return (
      <form className={classes.form}>
        <p>
          <label htmlFor="body">Enter Your Spouse Name here and Load the value in Parent Component</label>
          <textarea id="body" required rows={3} onChange={props.onAnyChangeEvent}/>
        </p>   
      </form>
    );
  }
  
  export default YourName;