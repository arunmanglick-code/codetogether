import { useParams } from 'react-router-dom';

function ProductDetailPage() {
  const params = useParams();

  return (
    <>
      <h1>Product Details!</h1>
      <p>Product Id: {params.productId}</p>
    </>
  );
}

export default ProductDetailPage;