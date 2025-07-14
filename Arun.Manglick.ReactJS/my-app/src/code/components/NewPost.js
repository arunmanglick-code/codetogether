import classes from '../../css/NewPost.module.css';
import { useState } from "react";

function NewPost() {

    const [myvar, setmyvar] = useState('');

    function myEventHandler(event)
    {
        // myvar = event.target.value;
        setmyvar(event.target.value);
    }

    return (
      <form className={classes.form}>
        <p>
          <label htmlFor="body">Enter Text in Same Component</label>
          <textarea id="body" required rows={3} onChange={myEventHandler} />
        </p>
        <p>You Entered: {myvar}</p>    
      </form>
    );
  }
  
  export default NewPost;