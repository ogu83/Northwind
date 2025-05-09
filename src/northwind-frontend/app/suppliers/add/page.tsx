"use client";

import { useParams, useRouter } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";
import Link from "next/link";

export default function AddSupplier() {
  const id  = 0;
  const router = useRouter();

const [supplier, setSupplier] = useState({
    supplierId: id,
    companyName: "",
    contactName: "",
    contactTitle: "",
    address: "",
    city: "",
    region: "",
    postalCode: "",
    country: "",
    phone: "",
    fax: "",
    homePage: ""
});

  const handleSubmit = async (e: { preventDefault: () => void; }) => {
    e.preventDefault();
    await axios.post("http://localhost:5205/Supplier", supplier);
    router.push("/suppliers");
  };

  return (
    <div className="p-4">
      <h1 className="text-xl font-semibold mb-4">Add Supplier</h1>
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
        <button type="submit" className="bg-green-500 text-white px-4 py-2 rounded">
          Save
        </button>
        <Link href="/suppliers" className="bg-blue-500 text-white px-4 py-2 rounded text-center">Cancel</Link>
      </form>
    </div>
  );
}
