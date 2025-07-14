import logo from './logo.svg';
import './App.css';
import HomePage from './pages/Home';
import ProductsPage from './pages/Product';
import RootPage from './pages/RootPage';
import ErrorPage from './pages/ErrorPage';
import ProductDetailPage from './pages/ProductDetail';

import { createBrowserRouter, RouterProvider, createRoutesFromElements, Route, errorElement } from 'react-router-dom';

// Method 1:
// const myrouter = createBrowserRouter([
//   { path: '/', element: <HomePage /> },
//   { path: '/products', element: <ProductPage /> },
// ]);

// Method 2:
// const routeDefinitions = createRoutesFromElements(
//   <Route>
//     <Route path="/" element={<HomePage />} />
//     <Route path="/products" element={<ProductPage />} />
//   </Route>
// );

// const myrouter = createBrowserRouter(routeDefinitions);

// Method 3: Nested Routes
const myrouter = createBrowserRouter([
  {
    path: '/',
    element: <RootPage />, 
    errorElement: <ErrorPage />,
    children: [
      // { path: '/', element: <HomePage /> },
      { index:true, element: <HomePage /> },
      { path: '/products', element: <ProductsPage /> },
      { path: '/products/:productId', element: <ProductDetailPage />}
    ],
  }
]);


function App() {
  // return (
  //   <div className="App">      
  //     <HomePage/>
  //     <p>Learn React Routing Mr. Manglick</p>
  //   </div>
  // );
  
  return <RouterProvider router={myrouter} />;
}

export default App;
