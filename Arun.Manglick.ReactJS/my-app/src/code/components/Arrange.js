import Tab from './Tabs';
import Basic from './Basic';
import NewPost from './NewPost';
import LearnProps from './LearnProps';
import YourName from './YourName';
import { useState } from "react";

function Arrange() {
    
    const [myvar, setmyvar] = useState('');

    function myEventHandler(event)
    {
        // myvar = event.target.value;
        setmyvar(event.target.value);
    }

    return (
    <table border={1}>
        <tr>
            <td colSpan={3}>
                <p>Your Wife Namee is: {myvar}</p>
                <YourName onAnyChangeEvent={myEventHandler} />
                
            </td>
        </tr>
        <tr>
            <td>
                <LearnProps firstName='Arun' lastName = 'Manglick'></LearnProps>
            </td>
            <td>
                <Basic />
            </td>
            <td>
                <NewPost />
            </td>
        </tr>
        <tr>
            <td colSpan={3}><Tab /></td>
        </tr>
    </table>
    );
}

export default Arrange;

