"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";

type Product = {
  productId: string;
  productName: string;
  supplierId: number;
  categoryId: number;
  quantityPerUnit: string;
  unitPrice: number;
  unitsInStock: number;
  unitsOnOrder: number;
  reorderLevel: number;
  discontinued: boolean;
};

type Supplier = {
  supplierId: number;
  companyName: string;
};

type Category = {
  categoryId: number,
  categoryName: string,
}

export default function AddProduct() {
  const id  = 0;
  const router = useRouter();

  const [product, setProduct] = useState<Product>({
    productId: id!,
    productName: "",
    supplierId: 0,
    categoryId: 0,
    quantityPerUnit: "",
    unitPrice: 0,
    unitsInStock: 0,
    unitsOnOrder: 0,
    reorderLevel: 0,
    discontinued: true,
  });

  const [suppliers, setSuppliers] = useState<Supplier[]>([]);
  const [categories, setCategories] = useState<Category[]>([]);

  // fetch suppliers list
  useEffect(() => {
    axios.get<Supplier[]>(`http://localhost:5205/Supplier`).then((res) => {
      setSuppliers(res.data);
    });
  }, []);

  // fetch suppliers list
  useEffect(() => {
    axios.get<Category[]>(`http://localhost:5205/Category`).then((res) => {
      setCategories(res.data);
    });
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await axios.post("http://localhost:5205/Product", product);
    router.push("/products");
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Edit Product {id}</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">
        <input
          type="text"
          className="border p-2"
          placeholder="Product Name"
          value={product.productName || ""}
          onChange={(e) =>
            setProduct({ ...product, productName: e.target.value })
          }
        />

        {/* Supplier Dropdown */}
        <select
          className="border p-2"
          value={product.supplierId}
          onChange={(e) =>
            setProduct({ ...product, supplierId: Number(e.target.value) })
          }
        >
          <option value={0} disabled>
            -- Select Supplier --
          </option>
          {suppliers.map((s) => (
            <option key={s.supplierId} value={s.supplierId}>
              {s.supplierId} – {s.companyName}
            </option>
          ))}
        </select>

        {/* Category Dropdown */}
        <select
          className="border p-2"
          value={product.categoryId}
          onChange={(e) =>
            setProduct({ ...product, categoryId: Number(e.target.value) })
          }
        >
          <option value={0} disabled>
            -- Select Catetory --
          </option>
          {categories.map((s) => (
            <option key={s.categoryId} value={s.categoryId}>
              {s.categoryId} – {s.categoryName}
            </option>
          ))}
        </select>

        <input
          type="text"
          className="border p-2"
          placeholder="Quantity Per Unit"
          value={product.quantityPerUnit || ""}
          onChange={(e) =>
            setProduct({ ...product, quantityPerUnit: e.target.value })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Unit Price"
          value={product.unitPrice || ""}
          onChange={(e) =>
            setProduct({ ...product, unitPrice: e.target.value })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Units In Stock"
          value={product.unitsInStock || ""}
          onChange={(e) =>
            setProduct({ ...product, unitsInStock: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Units On Order"
          value={product.unitsOnOrder || ""}
          onChange={(e) =>
            setProduct({ ...product, unitsOnOrder: e.target.value })
          }
        />

        <input
          type="number"
          className="border p-2"
          placeholder="Reorder Level"
          value={product.reorderLevel || ""}
          onChange={(e) =>
            setProduct({ ...product, reorderLevel: e.target.value })
          }
        />

        <label className="flex items-center gap-2">
          <input
            type="checkbox"
            checked={product.discontinued}
            onChange={(e) =>
              setProduct({ ...product, discontinued: e.target.checked })
            }
            className="h-5 w-5 text-blue-600 border-gray-300 rounded focus:ring-blue-500"
          />
          <span className="select-none">Discontinued</span>
        </label>

        <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
          Save
        </button>
      </form>
    </div>
  );
}
