"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";
import Link from "next/link";

type OrderDetail = {
    orderId: number,
    productId: number,
    unitPrice: number,
    quantity: number,
    discount: number
}

type Product = {
    productId: string;
    productName: string;
    unitPrice: number;
    unitsInStock: number;
    unitsOnOrder: number;
  };

export default function AddOrderDetails() {
  const { id } = useParams();
  const router = useRouter();

  const [products, setProducts] = useState<Product[]>([]);

  const [orderDetail, setOrderDetail] = useState<OrderDetail>({
    orderId: Number(id),
    productId: 0,
    unitPrice: 0.0,
    quantity: 0,
    discount: 0.0
  });

  // fetch products list
  useEffect(() => {
    axios.get<Product[]>(`http://localhost:5205/Product`).then((res) => {
        setProducts(res.data);
    });
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await axios.post("http://localhost:5205/OrderDetails", orderDetail);
    router.push(`/orders/edit/${id}`);
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Add Order: {id} Detail</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">

        {/* Product Dropdown */}
        <select
          className="border p-2"
          value={orderDetail.productId}
          onChange={(e) =>
            setOrderDetail({ ...orderDetail, productId: Number(e.target.value) })
          }
        >
          <option value={0} disabled>
            -- Select Product --
          </option>
          {products.map((s) => (
            <option key={s.productId} value={s.productId}>
              {s.productId} – {s.productName}
            </option>
          ))}
        </select>

        <input
          type="number"
          className="border p-2"
          placeholder="Unit Price"
          step="0.1"
          value={orderDetail.unitPrice || ""}
          onChange={(e) =>
            setOrderDetail({ ...orderDetail, unitPrice: Number(e.target.value) })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Quantity"
          value={orderDetail.quantity || ""}
          onChange={(e) =>
            setOrderDetail({ ...orderDetail, quantity: Number(e.target.value) })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Discount"
          step="0.1"
          value={orderDetail.discount || ""}
          onChange={(e) =>
            setOrderDetail({ ...orderDetail, discount: Number(e.target.value) })
          }
        />

        <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded">
          Save
        </button>
        <Link href={`/orders/edit/${id}/`} className="bg-blue-500 text-white px-4 py-2 rounded text-center">Cancel</Link>
      </form>
    </div>
  );
}
