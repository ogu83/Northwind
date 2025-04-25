"use client";

import { useQuery, useQueryClient, useMutation } from "@tanstack/react-query";
import { useParams, useRouter } from "next/navigation";
import axios from "axios";
import Link from "next/link";

export default function ProductsSupplier() {
  const queryClient = useQueryClient();
  const router = useRouter();
  const { id } = useParams();
  
  // Fetch products
  const { data: products, isLoading, error } = useQuery({
    queryKey: ["products"],
    queryFn: async () => {
      const res = await axios.get(`http://localhost:5205/Product/Supplier/${id}`);
      return res.data;
    },
  });

  // Delete mutation
  const deleteMutation = useMutation({
    mutationFn: async (id: number) => {
      await axios.delete(`http://localhost:5205/Product?id=${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["suppliers"] });
    },
    onError: (err) => {
      console.error("Error deleting supplier:", err);
      alert("Failed to delete supplier.");
    },
  });

  const handleDelete = (id: number) => {
    if (confirm(`Are you sure you want to delete supplier ${id}?`)) {
      deleteMutation.mutate(id);
    }
  };

  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading suppliers</div>;

  return (
    <div className="p-4">
        <h1 className="text-xl font-semibold mb-4">Products By Supplier {id}</h1>
        <div className="flex items-stretch mb-4">
            <Link className="text-blue-500 px-5 cursor-pointer" href="/">Home</Link>
            <Link className="text-blue-500 px-5 cursor-pointer" href="/products/add">Add New</Link>
        </div>
      <table className="table-auto w-full">
        <thead>
          <tr className="border-b">
          <th className="text-left p-2">ID</th>
            <th className="text-left p-2">Name</th>
            <th className="text-left p-2">Supplier Id</th>
            <th className="text-left p-2">Category Id</th>
            <th className="text-left p-2">Quantity Per Unit</th>
            <th className="text-left p-2">Unit Price</th>
            <th className="text-left p-2">Units In Stock</th>
            <th className="text-left p-2">UnitsOnOrder</th>
            <th className="text-left p-2">Reorder Level</th>
            <th className="text-left p-2">Discontinued</th>
            <th className="text-left p-2">Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.productId} className="border-b hover:bg-orange-950">
              <td className="p-1">{product.productId}</td>
              <td className="p-1">{product.productName}</td>
              <td className="p-1">
                <Link href={`/suppliers/edit/${product.supplierId}`} className="text-blue-500">{product.supplierId}</Link>
              </td>
              <td className="p-1">
                <Link href={`/categories/edit/${product.categoryId}`} className="text-blue-500">{product.categoryId}</Link>
              </td>
              <td className="p-1">{product.quantityPerUnit}</td>
              <td className="p-1">{product.unitPrice}</td>
              <td className="p-1">{product.unitsInStock}</td>
              <td className="p-1">{product.unitsOnOrder}</td>
              <td className="p-1">{product.reorderLevel}</td>
              <td className="p-1">{product.discontinued ? 'Yes' : 'No'}</td>
              <td className="p-1">
                <Link href={`/products/edit/${product.productId}`} className="text-blue-500 p-1">
                  Edit
                </Link>
                <button
                  className="text-red-500 p-1 cursor-pointer"
                  onClick={() => handleDelete(product.productId)}
                >
                  Delete
                </button>
            </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
