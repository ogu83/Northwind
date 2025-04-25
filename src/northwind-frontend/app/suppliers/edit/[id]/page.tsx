"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";

type Supplier = {
    supplierId: number,
    companyName: string,
    contactName: string,
    contactTitle: string,
    address: string,
    city: string,
    region: string,
    postalCode: string,
    country: string,
    phone: string,
    fax: string,
    homePage: string
};

export default function EditProduct() {
  const { id } = useParams();
  const router = useRouter();

  const [supplier, setSupplier] = useState<Supplier>({
    supplierId: id!,
    companyName: "",
    contactName: "",
    contactTitle: "",
    address: "",
    city: "",
    region: "",
    postalCode: "",
    country: "",
    phone: "",
    fax: ""
  });

  // fetch supplier
  useEffect(() => {
    axios.get(`http://localhost:5205/Supplier/${id}`).then((res) => {
      setSupplier(res.data);
    });
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    await axios.put("http://localhost:5205/Supplier", supplier);
    router.push("/suppliers");
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Edit Supplier {id}</h1>
      <form onSubmit={handleSubmit} className="flex flex-col gap-4 max-w-md">
        <input
          type="text"
          className="border p-2"
          placeholder="Company Name"
          value={supplier.companyName || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, companyName: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Contact Name"
          value={supplier.contactName || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, contactName: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Contact Title"
          value={supplier.contactTitle || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, contactTitle: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Address"
          value={supplier.address || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, address: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="City"
          value={supplier.city || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, city: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Region"
          value={supplier.region || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, region: e.target.value })
          }
        />
        
        <input
          type="text"
          className="border p-2"
          placeholder="Postal Code"
          value={supplier.postalCode || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, postalCode: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Country"
          value={supplier.country || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, country: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Phone"
          value={supplier.phone || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, phone: e.target.value })
          }
        />

        <input
          type="text"
          className="border p-2"
          placeholder="Fax"
          value={supplier.fax || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, fax: e.target.value })
          }
        />
        
        <input
          type="text"
          className="border p-2"
          placeholder="Home Page"
          value={supplier.homePage || ""}
          onChange={(e) =>
            setSupplier({ ...supplier, homePage: e.target.value })
          }
        />

        <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
          Save
        </button>
      </form>
    </div>
  );
}
