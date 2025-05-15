import requests
import random
import time
import concurrent.futures
import argparse

BASE_URL = "http://localhost:5205"

def list_orders(skip, take):
    response = requests.get(f"{BASE_URL}/Order/skip/{skip}/take/{take}")
    response.raise_for_status()
    return response.json()

def get_random_page(take):
    first_page = list_orders(0, take)
    total_count = first_page.get('totalCount', len(first_page.get('items', [])))
    page_count = first_page.get('pageCount', 1)
    page_index = random.randint(0, page_count - 1)
    skip = page_index * take
    page = list_orders(skip, take)
    return page

def update_order_random(order):
    original = order.copy()
    modified = order.copy()
    modified['freight'] = (modified.get('freight') or 0) + round(random.uniform(1, 10), 2)
    requests.put(f"{BASE_URL}/Order", json=modified).raise_for_status()
    return original

def create_order(order):
    new_order = order.copy()
    new_order['orderId'] = 0
    response = requests.post(f"{BASE_URL}/Order", json=new_order)
    response.raise_for_status()
    return response.json()

def find_order(order_id, take):
    skip = 0
    while True:
        page = list_orders(skip, take)
        items = page.get('items', page)
        for o in items:
            if o['orderId'] == order_id:
                return o
        skip += take
        if skip >= page.get('totalCount', skip):
            return None

def delete_order(order_id):
    requests.delete(f"{BASE_URL}/Order?id={order_id}").raise_for_status()

def user_story(take=10):
    page = get_random_page(take)
    order = random.choice(page.get('items', page)) if page else None
    if not order:
        return
        
    found = find_order(order["orderId"], take)

    # Step 2: Edit order
    original = update_order_random(order)
    time.sleep(0.1)
    
    # Step 3: Re-fetch and revert
    updated = requests.get(f"{BASE_URL}/Order/{order['orderId']}").json()
    requests.put(f"{BASE_URL}/Order", json=original).raise_for_status()
    
    # Step 4: Create new order
    created = create_order(updated)
    new_id = created['orderId']
    
    # Step 5: Find created order
    found = find_order(new_id, take)
    
    # Step 6: Delete the order
    delete_order(new_id)

def timed_user_story(iteration, take=10):
    start = time.time()
    try:
        user_story(take)
    except Exception as e:
        print(f"[Iteration {iteration}] Error: {e}")
    elapsed = (time.time() - start) * 1000  # ms
    print(f"[Iteration {iteration}] completed in {elapsed:.2f} ms")
    return elapsed

def main():
    p = argparse.ArgumentParser(description="")
    p.add_argument(
        "-c", "--concurrency",
        type=int,
        default=5,
        help="number of concurrent threads"
    )
    p.add_argument(
        "-i", "--iterations",
        type=int,
        default=10,
        help="total iterations per run"
    )
    args = p.parse_args()
    concurrency = args.concurrency;
    iterations = args.iterations;
    
    times = []
    with concurrent.futures.ThreadPoolExecutor(max_workers=concurrency) as executor:
        futures = {executor.submit(timed_user_story, i): i for i in range(iterations)}
        for future in concurrent.futures.as_completed(futures):
            elapsed = future.result()
            times.append(elapsed)
    
    if times:
        average = sum(times) / len(times)
        print(f"Average execution time: {average:.2f} ms over {len(times)} iterations")

if __name__ == "__main__":
    main()
