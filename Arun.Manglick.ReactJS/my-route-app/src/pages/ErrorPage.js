import GlobalNavigation from '../components/GlobalNavigation';
import classes from './Root.module.css';

function ErrorPage() {
  return (
    <>
      <GlobalNavigation />
      <main className={classes.content}>
        <h1>An error occurred!</h1>
        <p>Could not find this page!</p>
      </main>
    </>
  );
}

export default ErrorPage;