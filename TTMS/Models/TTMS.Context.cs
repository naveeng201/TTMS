﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TTMS.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TTMSEntities : DbContext
    {
        public TTMSEntities()
            : base("name=TTMSEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BrandsMaster> BrandsMasters { get; set; }
        public virtual DbSet<ProductCategory> ProductCategories { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendances { get; set; }
        public virtual DbSet<Order_Employee> Order_Employee { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<EmployeePerformance> EmployeePerformances { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<EmployeePayment> EmployeePayments { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<PurchaseEntry> PurchaseEntries { get; set; }
        public virtual DbSet<PurchaseEntryPayment> PurchaseEntryPayments { get; set; }
        public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<SaleProduct> SaleProducts { get; set; }
        public virtual DbSet<Order_Master> Order_Master { get; set; }
        public virtual DbSet<ProductsInventory> ProductsInventories { get; set; }
        public virtual DbSet<Order_Master_Items> Order_Master_Items { get; set; }
    }
}
