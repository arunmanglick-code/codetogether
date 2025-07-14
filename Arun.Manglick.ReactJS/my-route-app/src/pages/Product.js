import { Link, useNavigate } from 'react-router-dom';

const MyProducts = [
  { id: 'p1', title: 'Product 1' },
  { id: 'p2', title: 'Product 2' },
  { id: 'p3', title: 'Product 3' },
];

// function ProductPage() {
//   return <h1>My Product Page</h1>;
// }

function ProductsPage() {
  return (
    <>
      <h1>The Products Page</h1>
      <ul>
        {MyProducts.map((prod) => (
          <li key={prod.id}>
            <Link to={`/products/${prod.id}`}>{prod.title}</Link>
          </li>
        ))}
      </ul>
    </>
  );
}

export default ProductsPage;