function LearnProps(props) {
    return (
        <div className="App">
            <p>First Name: {props.firstName}</p>
            <p>Last Name: {props.lastName}</p>
        </div>
    );
}

export default LearnProps;