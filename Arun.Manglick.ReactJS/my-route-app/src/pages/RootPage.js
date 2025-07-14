import { Outlet } from 'react-router-dom';

import GlobalNavigation from '../components/GlobalNavigation';
import classes from './Root.module.css';

function RootPage() {
  return (
    <>
      <GlobalNavigation />
      <main className={classes.content}>
        <Outlet />
      </main>
    </>
  );
}

export default RootPage;