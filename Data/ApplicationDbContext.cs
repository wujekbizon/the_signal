using TheSignal.Models.Entities;
using TheSignal.Models.GameData;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TheSignal.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    // Game Entities
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationExit> LocationExits { get; set; }
    public DbSet<ExitCondition> ExitConditions { get; set; }
    
    public DbSet<Character> Characters { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<StatusEffect> StatusEffects { get; set; }
    
    public DbSet<Quest> Quests { get; set; }
    public DbSet<QuestObjective> QuestObjectives { get; set; }
    public DbSet<CharacterQuest> CharacterQuests { get; set; }
    
    public DbSet<GeneratedNarrative> GeneratedNarratives { get; set; }
    public DbSet<AudioLog> AudioLogs { get; set; }
    
    // AI Dungeon Master System
    public DbSet<ExplorationState> ExplorationStates { get; set; }
    public DbSet<ProceduralRoom> ProceduralRooms { get; set; }
    
    // Discovery System
    public DbSet<DiscoveredInteractable> DiscoveredInteractables { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure indexes for performance
        modelBuilder.Entity<GeneratedNarrative>()
            .HasIndex(gn => gn.ContextHash);
        
        modelBuilder.Entity<GeneratedNarrative>()
            .HasIndex(gn => gn.LocationId);
        
        modelBuilder.Entity<LocationExit>()
            .HasIndex(le => le.LocationId);
        
        modelBuilder.Entity<Character>()
            .HasIndex(c => c.CurrentLocationId);
        
        // Configure CharacterQuest relationships
        modelBuilder.Entity<CharacterQuest>()
            .HasOne(cq => cq.Character)
            .WithMany(c => c.Quests)
            .HasForeignKey(cq => cq.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure Location relationships
        modelBuilder.Entity<LocationExit>()
            .HasOne(le => le.Location)
            .WithMany(l => l.Exits)
            .HasForeignKey(le => le.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ExitCondition>()
            .HasOne(ec => ec.LocationExit)
            .WithMany(le => le.Conditions)
            .HasForeignKey(ec => ec.LocationExitId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure Character relationships
        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Character)
            .WithMany(c => c.Inventory)
            .HasForeignKey(ii => ii.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<StatusEffect>()
            .HasOne(se => se.Character)
            .WithMany(c => c.ActiveEffects)
            .HasForeignKey(se => se.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure Quest relationships
        modelBuilder.Entity<QuestObjective>()
            .HasOne(qo => qo.Quest)
            .WithMany(q => q.Objectives)
            .HasForeignKey(qo => qo.QuestId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<CharacterQuest>()
            .HasOne(cq => cq.Quest)
            .WithMany()
            .HasForeignKey(cq => cq.QuestId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Configure GeneratedNarrative relationship
        modelBuilder.Entity<GeneratedNarrative>()
            .HasOne(gn => gn.Location)
            .WithMany()
            .HasForeignKey(gn => gn.LocationId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Configure ExplorationState relationships
        modelBuilder.Entity<ExplorationState>()
            .HasOne(es => es.Character)
            .WithMany()
            .HasForeignKey(es => es.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ExplorationState>()
            .HasOne(es => es.PreviousRoom)
            .WithMany()
            .HasForeignKey(es => es.PreviousRoomId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<ExplorationState>()
            .HasIndex(es => es.CharacterId)
            .IsUnique();
        
        // Configure ProceduralRoom relationships
        modelBuilder.Entity<ProceduralRoom>()
            .HasOne(pr => pr.Character)
            .WithMany()
            .HasForeignKey(pr => pr.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProceduralRoom>()
            .HasIndex(pr => new { pr.CharacterId, pr.ContextHash });
        
        modelBuilder.Entity<ProceduralRoom>()
            .HasIndex(pr => pr.LastVisitedAt);
        
        // Configure DiscoveredInteractable relationships
        modelBuilder.Entity<DiscoveredInteractable>()
            .HasOne(di => di.Character)
            .WithMany(c => c.DiscoveredInteractables)
            .HasForeignKey(di => di.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<DiscoveredInteractable>()
            .HasIndex(di => new { di.CharacterId, di.InteractableId })
            .IsUnique();
        
        modelBuilder.Entity<DiscoveredInteractable>()
            .HasIndex(di => di.LocationId);
    }
}
